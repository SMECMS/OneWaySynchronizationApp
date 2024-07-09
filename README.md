# OneWaySynchronizationApp
A One-way Synchronization console App that fully syncs the contents of a folder to another.

## How it works

The Console App will first create all folders from source not yet present in destination,
then check all files in source to see if they are present in destination, if they are then a MD5 checksum decides if the destination file will be overwritten,
otherwise the file is copied from source to destination. Lastly it cleans the destination from all files and folders not present in source.
All this actions are logged onto a log file in path especified and onto the console and this cycle repeats every **X** seconds especified.

## How to use
on the Command Prompt execute the OneWaySynchronizationConsoleApp.exe passing 4 arguments:

+ **Source Path e.g.** (C:\Users\Foobar\Documents\MyImages\)
+ **Destination Path e.g.** (C:\Users\Foobar\Pictures\Screenshots\)
+ **Sync Cycle Interval**<sub> _In Seconds_</sub> **e.g.** (5)
+ **Log File Path e.g.** (C:\Users\Foobar\Pictures\Log\)

**_Very Important:_** all paths must end with ' \ ' and all paths that have spaces need to be inside double quotes ' " ' and end with double ' \ '.

### Examples:
```

OneWaySynchronizationConsoleApp.exe C:\Users\Foobar\Documents\MyImages\ C:\Users\Foobar\Pictures\Screenshots\ 5 C:\Users\Foobar\Pictures\Log\

```

```

OneWaySynchronizationConsoleApp.exe "C:\Users\Foobar\Documents\Path with spaces\\" C:\Users\Foobar\Pictures\Screenshots\ 5 "C:\Users\Foobar\Pictures\Another Path With Spaces\log\\"

```
