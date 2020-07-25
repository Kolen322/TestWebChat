FROM postgres:10.0
COPY /postgres/init.sql /docker-entrypoint-initdb.d/
EXPOSE 5432
VOLUME /var/lib/postgresql/data