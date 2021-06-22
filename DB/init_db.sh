#!/bin/bash

sleep 60

for i in {1..50};
do
    /opt/mssql-tools/bin/sqlcmd -S 127.0.0.1 -U SA -P ${SA_PASSWORD} -l 10 -d master -i db_init.sql
    if [ $? -eq 0 ]
    then
        echo "setup.sql completed"
        break
    else
        echo "not ready yet..."
        sleep 1s
    fi
done