Get Attachments in workspace
=========================

## Overview
These C# examples are based on [Rally Rest Toolkit for .NET](https://github.com/RallyTools/RallyRestToolkitFor.NET)
and tested with 3.0.1 dll

A common error when querying for attachments is related to specifics of TestCaseResult attachments.
In WS API Attachment object has Artifact attribute, which is a reference to any work item (except TestCaseResult) the attachment belongs, and TestCaseResult attribute, which is a reference to a TestCaseResult the attachment belongs.
When querying for all attachments in a workspace consider both scenarios. Traversing a query result object to Artifact's referernece will result in error if the attachment belongs to a TestCaseResult since Artifact is null.
The opposite will also result in error.

Also, TestCaseResult object in [WS API object model](https://rally1.rallydev.com/slm/doc/webservice/objectModel.sp) inherits directly from WorkspceDomanObject and does not have a Project attribute.
Work items such as UserStory(HierarchicalRequirement), Defect, TestCase inherit Project attribute from Artifact abstract type.
It means that scoping Attachment request to a project will not include attachments that belong to TestCaseResults.

This screenshot shows the console output if a TestCaseResult scenario is not taken into account when one of the query results is an attachment that belongs to a TCR

![](pic0.png)

This screenshot shows the console output after the code is fixed. Note that one of the query results belongs to a TCR:

![](pic1.png)

## License
These code examples are  available AS IS, for illustration purposes only. They are NOT supported by Rally.
AppTemplate is released under the MIT license.  See the file [LICENSE](./LICENSE) for the full text.

##Documentation for API toolkit

You can find the documentation on this [site.](https://github.com/RallyTools/RallyRestToolkitFor.NET)