FROM claims/mono-imagemagick:latest

ADD . /app
WORKDIR /app

CMD ["mono", "--debug", "Redis.ConnectionTest.exe"]
