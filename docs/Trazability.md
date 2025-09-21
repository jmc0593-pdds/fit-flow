## Trazability
### Logs and Traces

I used Sentry for the logging management application. The same configuration is already configured to use traces, so each event will be grouped with other events from other services.

#### Initialization on DI
```cs
builder.WebHost.UseSentry(options =>
    {
        options.Dsn = Environment.GetEnvironmentVariable("SENTRY_DSN");
        options.Debug = true;
        options.TracesSampleRate = 1;
        options.SendDefaultPii = true;
    });
```

#### Usage on normal messages
```cs
SentrySdk.CaptureMessage(message);
```

#### Usage on Exceptions
```cs
SentrySdk.CaptureException(message);
```