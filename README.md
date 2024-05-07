# ICUParserLib

[![](https://home.unicode.org/wp-content/uploads/2019/12/Unicode-Logo-Final-Blue-95x112.jpg)](https://icu.unicode.org/home)


[ICU Plural Rules](https://www.unicode.org/cldr/charts/45/supplemental/language_plural_rules.html)

[ANTLR](https://www.antlr.org/) 

> This repo has been populated by an initial template to help get you started. Please
> make sure to update the content to build a great experience for community-building.

<br/>

English, using 'one' and 'other' plural selector:
<pre>
{days, plural,
               one {Relaunch Microsoft Edge within # day}
               other {Relaunch Microsoft Edge within # days}}
</pre>
<br/>

French, using 'one', 'many' and 'other' plural selector:
<pre>
{days, plural,
               one {Relancez Microsoft Edge dans le # jour}
               many {Relancez Microsoft Edge dans les # jours}
               other {Relancez Microsoft Edge dans les # jours}}
</pre>
<br/>

Japanese, using only the 'other' plural selector:
<pre>
{days, plural,
               other {# 日以内に Microsoft Edge を再起動します}}
</pre>
<br/>

Arabic, using all plural selectors:
<pre>
{days, plural,
               one {أعد تشغيل Microsoft Edge خلال # one يوم}
               zero {أعد تشغيل Microsoft Edge في غضون # zero أيام}
               two {أعد تشغيل Microsoft Edge في غضون # two أيام}
               few {أعد تشغيل Microsoft Edge في غضون # few أيام}
               many {أعد تشغيل Microsoft Edge في غضون # many أيام}
               other {أعد تشغيل Microsoft Edge في غضون # other أيام}}
</pre>




Instantiate a ICUParser object:
```
string input = @"{days, plural,
    one {Relaunch Microsoft Edge within # day}
    other {Relaunch Microsoft Edge within # days}}";

ICUParser icuParser = new ICUParser(input, true);
```

The library parses the input and segments the content to all possible plural selectors. Additional selectors gets copied from the 'other' selector.

```
List<MessageItem> messageItems = icuParser.GetMessageItems();
```

    messageItems[0].Plural="one"
    messageItems[0].Text="Relaunch Microsoft Edge within # day"

    messageItems[1].Plural="other"
    messageItems[1].Text="Relaunch Microsoft Edge within # days"

    messageItems[2].Plural="zero"
    messageItems[2].Text="Relaunch Microsoft Edge within # days"

    messageItems[3].Plural="two"
    messageItems[3].Text="Relaunch Microsoft Edge within # days"

    messageItems[4].Plural="few"
    messageItems[4].Text="Relaunch Microsoft Edge within # days"

    messageItems[5].Plural="many"
    messageItems[5].Text="Relaunch Microsoft Edge within # days"


The message segments gets translated and the final ICU message block gets composed:

```
// Composed string for 'fr'.
string output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("fr"));
```
<br/>

<pre>
{days, plural,
               one {Relancez Microsoft Edge dans le # (translated for one) jour}
               many {Relancez Microsoft Edge dans les # (translated for many) jours}
               other {Relancez Microsoft Edge dans les # (translated for other) jours}}
</pre>

<br/>

## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Trademarks

This project may contain trademarks or logos for projects, products, or services. Authorized use of Microsoft 
trademarks or logos is subject to and must follow 
[Microsoft's Trademark & Brand Guidelines](https://www.microsoft.com/en-us/legal/intellectualproperty/trademarks/usage/general).
Use of Microsoft trademarks or logos in modified versions of this project must not cause confusion or imply Microsoft sponsorship.
Any use of third-party trademarks or logos are subject to those third-party's policies.
