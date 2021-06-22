FROM mcr.microsoft.com/mssql/server

COPY init_db.sh .
COPY entrypoint.sh .
COPY main/db_init.sql .

CMD ["/bin/sh", "./entrypoint.sh"]
