FROM mcr.microsoft.com/mssql/server:latest
LABEL MAINTAINER=DNMOFT
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=brephesAc3CUbu!
ENV MSSQL_PID=Enterprise
ENV MSSQL_TCP_PORT=1433 
WORKDIR /src
RUN (/opt/mssql/bin/sqlservr --accept-eula & ) | grep -q "Service Broker manager has started" &&  /opt/mssql-tools/bin/sqlcmd -S 127.0.0.1 -U sa -P brephesAc3CUbu! -Q "CREATE DATABASE dbDGII; exec sp_configure 'show advanced options',1; reconfigure; exec sp_configure 'Agent XPs',1; reconfigure; exec sp_configure 'remote admin connections',1; reconfigure;"