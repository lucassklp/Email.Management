<img src="Resources/logo-complete.png">

### Send e-mails, create and edit templates using Rest API

## How to run

### Define a Secret

Before running this service, you must setup your secret. This secret is used as encryption key in order to save your email credentials on database. **Be careful: don't use the default value.**

To do that, go to [docker-compose.yaml](./docker-compose.yaml) and change the following value:

```yaml
environment:
    JwtSecret: "change this value" # This line is your Jwt Token symetric key
    PasswordSecret: "change this value" # This line is your Password Secret Key (used to encrypt your password on database)
```

### Running

After running the code below, you can access the service by accessing http://localhost:4000

```sh
docker compose up -d
```

## Check the endpoints definitions

After running, you can see the endpoint definitions in http://localhost:4000/swagger/index.html