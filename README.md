# TemplateFileRunner

## Description
This application aims to allow for batch creation of files based off a template.

## How to Configure
1. Create your template file. It should include 0-indexed values as placeholders. You can reuse indexes throughout the file. Placeholders should surround indices with curly braces ```{}```. An example template file is shown below.
```txt
Hello {0},

After reviewing your profile, we've think that your experience in {1} would make you a great candidate for {2}! Our offer is as follows:

{3}

For more information, please visit {4}

{0}, don't miss out on this once in a lifetime opportunity!

Thanks,
{5}
```
2. Add your template file to the "Resources" folder
3. Update the appsettings.json file with the template file name, and values for each output file. If running outside of Visual Studio, ensure 'ExecutedFromSolution' is set to ```false```.  The following charts describes each value, while an example appsettings.json and ReplacementProperties files are shown for reference

| Value | Description |
| --    | -- |
| ResourceDirectory | The folder within this project that all files will be read/written. |
|  TemplateName | Name of the template file to be read. |
|  OutputDirectory | Subfolder within the 'ResourceDirectory' to write all the output files to. |
|  OutputBaseName | A name that all output files will share.  |
|  OutputFileExtension | File extension of the output files.  |

> | Per Output-file value | Description |
> | --    | -- |
> | Prefix | text to be prepended to this specific output file name  |
> |  Suffix | text to be appended to this specific output file name |
> |  AppendDateTimeSuffix | This will add the DateTime at the end of the file name. This prevents duplicate file names. It also prevents overriding files during different executions |
> |  AppendCounterSuffix | This will add an index (starting at 0) at the end of the file name. This prevents duplicate file names.  |
> |  Values | Names do not matter (so you can make them descriptive for your use case), as long as they match the FileTemplateSettings.ReplacementProperties class. <br><br>If you need more/less values, add/remove from the ReplacementProperties class. <br><br>For newlines, use ```\r\n``` and for tabs use ```\t``` |

appsettings.json:
```json
{
  "FileTemplateSettings": {
    "ResourceDirectory": "Resources",
    "TemplateName": "templateFile.txt",
    "OutputDirectory": "Generated",
    "OutputBaseName": "OfferLetter",
    "OutputFileExtension": "txt",
    "OutputFiles": [
      {
        "Prefix": "Sean",
        "Suffix": "ComputerScience",
        "AppendDateTimeSuffix": true,
        "ReplacementProperties": {
          "Value0": "Sean",
          "Value1": "Computer Science at NYIT",
          "Value2": "our Software Developer position",
          "Value3": "8am - 6pm daily. Starting Salary 15k",
          "Value4": "https://jobsearcher.com/j/software-engineer-remote-welcome-up-to-15k-starting-bonus-military-veterans-at-highmark-health-in-juneau-ak-Opq6lGO",
          "Value5": "Joseph Cremler\r\nHead of Talent Acquisition"
        }
      },
      {
        "Suffix": "Business",
        "AppendCounterSuffix": true,
        "ReplacementProperties": {
          "Value0": "John",
          "Value1": "Business Analytics",
          "Value2": "the McDonald's Manager position",
          "Value3": "Coordinate staffing schedules. Flexible schedule, minimum of 30hr/week. Starting Salary 45k",
          "Value4": "https://www.linkedin.com/jobs/view/shift-manager-at-mcdonald-s-2622734143/?utm_campaign=google_jobs_apply&utm_source=google_jobs_apply&utm_medium=organic",
          "Value5": "Joseph Cremler\r\nHead of Talent Acquisition"
        }
      }
    ]
  },
  "EnvironmentSettings": {
    "ExecutedFromSolution": true
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Warning"
    }
  }
}
```

ReplacementProperties:
```c#
public class ReplacementProperties
{
    public string Value0 { get; set; }
    public string Value1 { get; set; }
    public string Value2 { get; set; }
    public string Value3 { get; set; }
    public string Value4 { get; set; }
    public string Value5 { get; set; }

    . . .

    //Class Helper methods
}
```


This example should result in two files being created. Their outputs are as followings:


SeanOfferLetterComputerScience20220304-224514-0460892.txt:
```txt
Hello Sean,

After reviewing your profile, we've think that your experience in Computer Science at NYIT would make you a great candidate for our Software Developer position! Our offer is as follows:

8am - 6pm daily. Starting Salary 15k

For more information, please visit https://jobsearcher.com/j/software-engineer-remote-welcome-up-to-15k-starting-bonus-military-veterans-at-highmark-health-in-juneau-ak-Opq6lGO

Sean, don't miss out on this once in a lifetime opportunity!

Thanks,
Joseph Cremler
Head of Talent Acquisition
```

OfferLetterBusiness1.txt:
```txt
Hello John,

After reviewing your profile, we've think that your experience in Business Analytics would make you a great candidate for the McDonald's Manager position! Our offer is as follows:

Coordinate staffing schedules. Flexible schedule, minimum of 30hr/week. Starting Salary 45k

For more information, please visit https://www.linkedin.com/jobs/view/shift-manager-at-mcdonald-s-2622734143/?utm_campaign=google_jobs_apply&utm_source=google_jobs_apply&utm_medium=organic

John, don't miss out on this once in a lifetime opportunity!

Thanks,
Joseph Cremler
Head of Talent Acquisition
```

## Usage

### Visual Studio
When using Visual Studio, simply execute the project.

### Command Line Interface (CLI)
When using Visual Studio Code (or any commandline-based approach), ensure you are in the folder with the "TemplateFilerRunner.csproj" file, then execute
```
dotnet run
```

## Validation
The program will attempt to validation your configuration before execution. Any errors will be written to the console. Notes about validation
- Validation will prevent duplicate output file names from being created
- Validation will ensure all template replacement values are defined (Not Null or Whitespace)