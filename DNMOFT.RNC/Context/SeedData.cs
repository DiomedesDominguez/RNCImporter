using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace DNMOFT.RNC.Context
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AppContext(serviceProvider.GetRequiredService<DbContextOptions<AppContext>>()))
            {
                var firstRecord = context.mContribuyentes.FirstOrDefault();
                if (firstRecord != null && firstRecord.Actualizado.Date == DateTime.Today)
                {
                    return;
                }

                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                const string zipPath = "DGII_RNC.zip";
                var extractPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                const string zipUrl = "http://www.dgii.gov.do/app/WebApps/Consultas/RNC/DGII_RNC.zip";
                using (var httpClient = new WebClient())
                {
                    httpClient.DownloadFile(new Uri(zipUrl), zipPath);
                    ZipFile.ExtractToDirectory(zipPath, extractPath);
                }

                context.Database.ExecuteSqlRaw("TRUNCATE TABLE mContribuyentes;");
                const int batchSize = 100000;

                var filePath = Path.Combine(extractPath, "TMP", "DGII_RNC.TXT");
                using (var sReader = new StreamReader(filePath, Encoding.Default))
                {
                    var listRnc = new List<mContribuyente>();

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

                            listRnc.Add(new mContribuyente(sLine));
                        }

                        iBatchsize += 1;
                        if (iBatchsize != batchSize)
                        {
                            continue;
                        }

                        context.mContribuyentes.AddRange(listRnc);
                        context.SaveChanges();
                        listRnc.Clear();
                    }
                    context.mContribuyentes.AddRange(listRnc);
                    context.SaveChanges();
                    listRnc.Clear();
                }
                File.Delete(filePath);
                File.Delete(zipPath);
            }
        }
    }
}
