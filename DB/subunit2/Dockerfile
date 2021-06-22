FROM mcr.microsoft.com/mssql/server

COPY init_db.sh .
COPY entrypoint.sh .
COPY subunit2/db_init.sql .

CMD ["/bin/sh", "./entrypoint.sh"]
