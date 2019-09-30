// ***********************************************************************
// Assembly         : RNC Importer
// Author           : Diomedes Domínguez
// Created          : 2017-07-15
//
// Last Modified By : Diomedes Domínguez
// Last Modified On : 2019-09-30
// ***********************************************************************
// <copyright file="Program.cs" company="DN MOFT">
//     Copyright © DN MOFT 2017
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DNMOFT.RNCI
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
    using System.Net;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// Class Program.
    /// </summary>
    internal class Program
    {
        #region Methods

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            var oTimer = new Stopwatch();
            oTimer.Start();

            WriteLogger("========================INICIO DEL LOG========================");
            try
            {
                const string zipPath = "DGII_RNC.zip";
                var extractPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (!Directory.Exists(Path.Combine(extractPath, "TMP")))
                {
                    const string zipUrl = "http://www.dgii.gov.do/app/WebApps/Consultas/RNC/DGII_RNC.zip";
                    WriteLogger("Descargando la ultima version del archivo zip...");

                    var httpClient = new WebClient();
                    httpClient.DownloadFile(new Uri(zipUrl), zipPath);

                    oTimer.Stop();
                    WriteLogger($"Descargado en {oTimer.Elapsed.TotalSeconds:N2} segundos.");
                    oTimer.Reset();

                    oTimer.Start();

                    ZipFile.ExtractToDirectory(zipPath, extractPath);

                }

                var rows = 0;
                var sConn = ConfigurationManager.AppSettings["sqlConn"];
                var sTable = ConfigurationManager.AppSettings["tableName"];
                int.TryParse(ConfigurationManager.AppSettings["batchSize"], out var batchSize);

                using (var sqlConnection = new SqlConnection(sConn))
                {
                    using (var sqlCommand = new SqlCommand("DELETE FROM " + sTable, sqlConnection))
                    {
                        sqlConnection.Open();
                        sqlCommand.CommandType = CommandType.Text;
                        sqlCommand.ExecuteNonQuery();
                        sqlConnection.Close();
                    }

                    using (var sqlBulk = new SqlBulkCopy(sqlConnection.ConnectionString, SqlBulkCopyOptions.TableLock))
                    {
                        sqlBulk.BatchSize = batchSize <= 1000 ? 10000 : batchSize;
                        sqlBulk.DestinationTableName = sTable;
                        sqlBulk.BulkCopyTimeout = 0;

                        var filePath = Path.Combine(extractPath, "TMP", "DGII_RNC.TXT");
                        using (var sReader = new StreamReader(filePath, Encoding.Default))
                        {
                            using (var dtRnc = new DataTable(sTable))
                            {
                                dtRnc.Columns.Add("RNC");
                                dtRnc.Columns.Add("RazonSocial");
                                dtRnc.Columns.Add("NombreComercial");
                                dtRnc.Columns.Add("Categoria");
                                dtRnc.Columns.Add("CalleAvenida");
                                dtRnc.Columns.Add("Numero");
                                dtRnc.Columns.Add("Sector");
                                dtRnc.Columns.Add("Telefono");
                                dtRnc.Columns.Add("Registrado");
                                dtRnc.Columns.Add("Estado");
                                dtRnc.Columns.Add("Regimen");

                                var iBatchsize = 0;
                                while (!sReader.EndOfStream)
                                {
                                    var readLine = sReader.ReadLine();
                                    if (readLine != null)
                                    {
                                        var sLine = readLine.Split('|');

                                        for (var i = 0; i < sLine.Length; i++)
                                        {
                                            sLine[i] = sLine[i].Trim();
                                            while (sLine[i].Contains("  "))
                                            {
                                                sLine[i] = sLine[i].Replace("  ", " ");
                                            }
                                        }

                                        dtRnc.Rows.Add(sLine);
                                    }
                                    iBatchsize += 1;
                                    rows += 1;
                                    if (iBatchsize != sqlBulk.BatchSize) continue;
                                    sqlBulk.WriteToServer(dtRnc);
                                    dtRnc.Rows.Clear();

                                    Console.WriteLine($"Insertando {iBatchsize} registros");
                                    iBatchsize = 0;
                                }
                                sqlBulk.WriteToServer(dtRnc);
                                dtRnc.Rows.Clear();
                                Console.WriteLine($"Insertando {iBatchsize} registros");
                            }
                        }
                        File.Delete(filePath);
                    }
                    File.Delete(zipPath);
                }

                oTimer.Stop();
                WriteLogger($"{rows:N0} registros importados en {oTimer.Elapsed.TotalSeconds:N2} segundos.");
            }
            catch (Exception ex)
            {
                WriteLogger(ex.ToString());
            }
            WriteLogger("=========================FIN DEL LOG==========================");
        }

        /// <summary>
        /// Writes the logger.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void WriteLogger(string message)
        {
            using (var sw = new StreamWriter("log.txt", true))
            {
                sw.WriteLine($"{DateTime.Now:yyyyMMddHHmmss} - {message}");
            }
            Console.WriteLine(message);
        }

        #endregion Methods
    }
}