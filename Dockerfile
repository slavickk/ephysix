FROM python:3.10-slim
WORKDIR /app
ENV PYTHONDONTWRITEBYTECODE 1
ENV PYTHONUNBUFFERED 1
COPY requirements.txt /app
RUN apt-get update && \
    apt-get -y install libpq-dev gcc procps libaio1 wget unzip && \
    wget -q https://download.oracle.com/otn_software/linux/instantclient/instantclient-basiclite-linuxx64.zip && \
    unzip instantclient-basiclite-linuxx64.zip && \
    rm -f instantclient-basiclite-linuxx64.zip && \
    cd instantclient* && \
    rm -f *jdbc* *occi* *mysql* *jar uidrvci genezi adrci && \
    echo /app/instantclient* > /etc/ld.so.conf.d/oracle-instantclient.conf && \
    ldconfig && \
    cd .. && \
    python -m pip install --upgrade pip && \
    pip install --no-cache-dir -r requirements.txt
COPY . /app
CMD ["python", "/app/main.py"]
