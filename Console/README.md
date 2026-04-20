# DevExpress Office File API - Process DOCX Documents, XLSX Spreadsheets, PPTX Presentations, and PDF Files in .NET Console Apps

This repository contains console applications that use the DevExpress Office File API to process Microsoft Word, Excel, PowerPoint documents and PDF files. Included examples implement common document management scenarios: 

- Convert a document to another format.
- Add or extract content.
- Digitally sign or protect a document.
- Use Mail Merge for bulk document generation.
- Modify document properties.

> Office, Excel, Word, PowerPoint are trademarks or registered trademarks of Microsoft Corporation.

## Process PDF Documents

[DevExpress PDF Document API for .NET](https://www.devexpress.com/products/net/office-file-api/pdf/) allows you to generate, edit, convert, merge, split, password-protect, and digitally sign PDF files.

For additional information, refer to product documentation: [DevExpress PDF Document API](https://docs.devexpress.com/OfficeFileAPI/16491/pdf-document-api).

### Convert a PDF Document to PDF/A

Converts a PDF file to PDF/A format.

**Project folder**: *PdfProcessing* / *DocumentConversion* / *PdfToCompatible*  
**Files to review**: [Program.cs](./CS/PdfProcessing/DocumentConversion/PdfToCompatible/Program.cs) | [Program.vb](./VB/PdfProcessing/DocumentConversion/PdfToCompatible/Program.vb)

### Convert a PDF Document to Images

Exports PDF document pages as images.

**Project folder**: *PdfProcessing* / *DocumentConversion* / *PdfToImage*  
**Files to review**: [Program.cs](./CS/PdfProcessing/DocumentConversion/PdfToImage/Program.cs) | [Program.vb](./VB/PdfProcessing/DocumentConversion/PdfToImage/Program.vb)

### Add a Watermark to a PDF Document

Adds a text watermark to a PDF document.

**Project folder**: *PdfProcessing* / *DocumentProtection* / *PdfApplyWatermark*  
**Files to review**: [Program.cs](./CS/PdfProcessing/DocumentProtection/PdfApplyWatermark/Program.cs) | [Program.vb](./VB/PdfProcessing/DocumentProtection/PdfApplyWatermark/Program.vb)

### Sign a PDF Document

Signs a PDF document with a predefined certificate and applies signature appearance settings.

**Project folder**: *PdfProcessing* / *DocumentProtection* / *PdfDigitalSignature*  
**Files to review**: [Program.cs](./CS/PdfProcessing/DocumentProtection/PdfDigitalSignature/Program.cs) | [Program.vb](./VB/PdfProcessing/DocumentProtection/PdfDigitalSignature/Program.vb)

### Encrypt a PDF Document

Encrypts a PDF document using the 256-bit AES algorithm and sets permissions (all Allowed).

**Project folder**: *PdfProcessing* / *DocumentProtection* / *PdfEncryptPassword*  
**Files to review**: [Program.cs](./CS/PdfProcessing/DocumentProtection/PdfEncryptPassword/Program.cs) | [Program.vb](./VB/PdfProcessing/DocumentProtection/PdfEncryptPassword/Program.vb)

### Redact a PDF Document

Adds and applies redaction annotations to a PDF document.

**Project folder**: *PdfProcessing* / *DocumentProtection* / *PdfRedaction*  
**Files to review**: [Program.cs](./CS/PdfProcessing/DocumentProtection/PdfRedaction/Program.cs) | [Program.vb](./VB/PdfProcessing/DocumentProtection/PdfRedaction/Program.vb)

### Validate Signatures in a PDF Document

Validates signatures in a signed PDF document and writes a text report (TXT).

**Project folder**: *PdfProcessing* / *DocumentProtection* / *PdfValidateSignature*  
**Files to review**: [Program.cs](./CS/PdfProcessing/DocumentProtection/PdfValidateSignature/Program.cs) | [Program.vb](./VB/PdfProcessing/DocumentProtection/PdfValidateSignature/Program.vb)

### Merge PDF Documents

Merges multiple PDF documents into a single PDF document.

**Project folder**: *PdfProcessing* / *DocumentConversion* / *PdfMergeDocuments*  
**Files to review**: [Program.cs](./CS/PdfProcessing/DocumentConversion/PdfMergeDocuments/Program.cs) | [Program.vb](./VB/PdfProcessing/MergeSplit/PdfMergeDocuments/Program.vb)

### Split a PDF Document

Splits a PDF document into multiple single-page PDF documents.

**Project folder**: *PdfProcessing* / *MergeSplit* / *PdfSplitDocuments*  
**Files to review**: [Program.cs](./CS/PdfProcessing/MergeSplit/PdfSplitDocuments/Program.cs) | [Program.vb](./VB/PdfProcessing/MergeSplit/PdfSplitDocuments/Program.vb)


## Process Word Documents

[DevExpress Word (RTF) Document API](https://www.devexpress.com/products/net/office-file-api/word/) allows you to to create, read, edit, convert, merge, split, password-protect, and digitally sign Microsoft Word documents.

For additional information, refer to product documentation: [Word Processing Document API](https://docs.devexpress.com/OfficeFileAPI/17488/word-processing-document-api).

### Convert Word Document Pages to Images

Exports Word document (DOCX) pages as PNG images.

**Project folder**: *WordProcessing* / *DocumentConversion* / *WordToImage*  
**Files to review**: [Program.cs](./CS/WordProcessing/DocumentConversion/WordToImage/Program.cs) | [Program.vb](./VB/WordProcessing/DocumentConversion/WordToImage/Program.vb)

### Convert a Word Document to PDF

Converts a Word document (DOCX) to PDF format.

**Project folder**: *WordProcessing* / *DocumentConversion* / *WordToPdf*  
**Files to review**: [Program.cs](./CS/WordProcessing/DocumentConversion/WordToPdf/Program.cs) | [Program.vb](./VB/WordProcessing/DocumentConversion/WordToPdf/Program.vb)

### Convert a Word Document to Another Format

Converts a Word document (DOCX) to the specified format.

**Project folder**: *WordProcessing* / *DocumentConversion* / *WordToWord*  
**Files to review**: [Program.cs](./CS/WordProcessing/DocumentConversion/WordToWord/Program.cs) | [Program.vb](./VB/WordProcessing/DocumentConversion/WordToWord/Program.vb)

### Add a Digital Signature to a Word Document 

Digitally signs a Word document with a predefined certificate.

**Project folder**: *WordProcessing* / *DocumentProtection* / *WordDigitalSignature*  
**Files to review**: [Program.cs](./CS/WordProcessing/DocumentProtection/WordDigitalSignature/Program.cs) | [Program.vb](./VB/WordProcessing/DocumentProtection/WordDigitalSignature/Program.vb)

### Encrypt a Word Document

Encrypts and password-protects a Word document (DOCX).

**Project folder**: *WordProcessing* / *DocumentProtection* / *WordEncryptPassword*  
**Files to review**: [Program.cs](./CS/WordProcessing/DocumentProtection/WordEncryptPassword/Program.cs) | [Program.vb](./VB/WordProcessing/DocumentProtection/WordEncryptPassword/Program.vb)

### Restrict Word Document Modification

Protects a Word document (DOCX) from unauthorized modifications (specifies an editing restriction password).

**Project folder**: *WordProcessing* / *DocumentProtection* / *WordProtectPassword*  
**Files to review**: [Program.cs](./CS/WordProcessing/DocumentProtection/WordProtectPassword/Program.cs) | [Program.vb](./VB/WordProcessing/DocumentProtection/WordProtectPassword/Program.vb)

### Mail Merge

Fills a predefined mail-merge template with data to create personalized letters.

**Project folder**: *WordProcessing* / *MailMergeFields* / *WordMailMerge*  
**Files to review**: [Program.cs](./CS/WordProcessing/MailMergeFields/WordMailMerge/Program.cs) | [Program.vb](./VB/WordProcessing/MailMergeFields/WordMailMerge/Program.vb)

### Define Master-Detail Regions in a Template

Generates an order report from a predefined master-detail mail-merge template.

**Project folder**: *WordProcessing* / *MailMergeFields* / *WordMailMergeRegions*  
**Files to review**: [Program.cs](./CS/WordProcessing/MailMergeFields/WordMailMergeRegions/Program.cs) | [Program.vb](./VB/WordProcessing/MailMergeFields/WordMailMergeRegions/Program.vb)

### Merge Word Documents

Merges multiple Word documents (DOCX) into a single document (PDF).

**Project folder**: *WordProcessing* / *MergeSplit* / *Merge*  
**Files to review**: [Program.cs](./CS/WordProcessing/MergeSplit/Merge/Program.cs) | [Program.vb](./VB/WordProcessing/MergeSplit/Merge/Program.vb)

### Split a Word Document

Splits a Word document (DOCX) into multiple documents by page, section, or heading. You can specify the output format.

**Project folder**: *WordProcessing* / *MergeSplit* / *Split*  
**Files to review**: [Program.cs](./CS/WordProcessing/MergeSplit/Split/Program.cs) | [Program.vb](./VB/WordProcessing/MergeSplit/Split/Program.vb)


## Process Excel Spreadsheets

[DevExpress Excel Spreadsheet API for .NET](https://www.devexpress.com/products/net/office-file-api/spreadsheet/) allows you to create, read, write, convert, print, and protect Microsoft Excel files.

For additional information, refer to product documentation: [Spreadsheet Document API](https://docs.devexpress.com/OfficeFileAPI/14912/spreadsheet-document-api).


### Convert an Excel Spreadsheet to Another Format

Converts an XLSX spreadsheet to the specified spreadsheet format.

**Project folder**: *Spreadsheet* / *DocumentConversion* / *ExcelToExcel*  
**Files to review**: [Program.cs](./CS/Spreadsheet/DocumentConversion/ExcelToExcel/Program.cs) | [Program.vb](./VB/Spreadsheet/DocumentConversion/ExcelToExcel/Program.vb)

### Convert an Excel Spreadsheet to PDF

Converts an XLSX spreadsheet to PDF format.

**Project folder**: *Spreadsheet* / *DocumentConversion* / *ExcelToPdf*  
**Files to review**: [Program.cs](./CS/Spreadsheet/DocumentConversion/ExcelToPdf/Program.cs) | [Program.vb](./VB/Spreadsheet/DocumentConversion/ExcelToPdf/Program.vb)

### Convert an Excel Spreadsheet to Images

Exports XLSX spreadsheet worksheets as PNG images.

**Project folder**: *Spreadsheet* / *DocumentConversion* / *ExcelToImage*  
**Files to review**: [Program.cs](./CS/Spreadsheet/DocumentConversion/ExcelToImage/Program.cs) | [Program.vb](./VB/Spreadsheet/DocumentConversion/ExcelToImage/Program.vb)

### Convert an Excel Worksheet to HTML

Converts an XLSX spreadsheet worksheet to HTML format.

**Project folder**: *Spreadsheet* / *DocumentConversion* / *ExcelToHtml*  
**Files to review**: [Program.cs](./CS/Spreadsheet/DocumentConversion/ExcelToHtml/Program.cs) | [Program.vb](./VB/Spreadsheet/DocumentConversion/ExcelToHtml/Program.vb)

### Add a Digital Signature to an Excel Spreadsheet

Digitally signs an XLSX spreadsheet with a predefined certificate.

**Project folder**: *Spreadsheet* / *DocumentProtection* / *ExcelDigitalSignature*  
**Files to review**: [Program.cs](./CS/Spreadsheet/DocumentProtection/ExcelDigitalSignature/Program.cs) | [Program.vb](./VB/Spreadsheet/DocumentProtection/ExcelDigitalSignature/Program.vb)

### Encrypt an Excel Spreadsheet

Encrypts and password-protects an XLSX spreadsheet.

**Project folder**: *Spreadsheet* / *DocumentProtection* / *ExcelEncryptPassword*  
**Files to review**: [Program.cs](./CS/Spreadsheet/DocumentProtection/ExcelEncryptPassword/Program.cs) | [Program.vb](./VB/Spreadsheet/DocumentProtection/ExcelEncryptPassword/Program.vb)

### Restrict Excel Worksheet Modification

Protects worksheet cell content from modifications (adds an editing restriction password).

**Project folder**: *Spreadsheet* / *DocumentProtection* / *ExcelProtectWorkbook*  
**Files to review**: [Program.cs](./CS/Spreadsheet/DocumentProtection/ExcelProtectWorkbook/Program.cs) | [Program.vb](./VB/Spreadsheet/DocumentProtection/ExcelProtectWorkbook/Program.vb)

### Restrict Excel Workbook Modification

Protects a workbook from structural changes (sets a password for add/delete/rename worksheet operations). 

**Project folder**: *Spreadsheet* / *DocumentProtection* / *ExcelProtectWorksheet*  
**Files to review**: [Program.cs](./CS/Spreadsheet/DocumentProtection/ExcelProtectWorksheet/Program.cs) | [Program.vb](./VB/Spreadsheet/DocumentProtection/ExcelProtectWorksheet/Program.vb)

### Merge Excel Spreadsheets

Merges multiple XLSX spreadsheets into a single spreadsheet.

**Project folder**: *Spreadsheet* / *MergeSplitDocuments* / *ExcelMergeWorkbooks*  
**Files to review**: [Program.cs](./CS/Spreadsheet/MergeSplitDocuments/ExcelMergeWorkbooks/Program.cs) | [Program.vb](./VB/Spreadsheet/MergeSplitDocuments/ExcelMergeWorkbooks/Program.vb)

### Split an Excel Spreadsheet

Splits an XLSX spreadsheet into multiple single-sheet spreadsheets or PDF documents.

**Project folder**: *Spreadsheet* / *MergeSplitDocuments* / *ExcelSplitWorkbooks*  
**Files to review**: [Program.cs](./CS/Spreadsheet/MergeSplitDocuments/ExcelSplitWorkbooks/Program.cs) | [Program.vb](./VB/Spreadsheet/MergeSplitDocuments/ExcelSplitWorkbooks/Program.vb)

## Process PowerPoint Presentations

[DevExpress PowerPoint Presentation API for .NET](https://www.devexpress.com/products/net/office-file-api/powerpoint-presentation-api/) allows you to generate, edit, merge, print/export, and digitally sign Microsoft PowerPoint® files.

For additional information, refer to product documentation: [DevExpress Presentation API](https://docs.devexpress.com/OfficeFileAPI/405405/presentation-api/overview).

### Convert a Presentation to PDF

Converts a PPTX presentation to PDF format.

**Project folder**: *PowerPointPresentation* / *DocumentConversion* / *PresentationToPdf*  
**Files to review**: [Program.cs](./CS/PowerPointPresentation/DocumentConversion/PresentationToPdf/Program.cs) | [Program.vb](./VB/PowerPointPresentation/DocumentConversion/PresentationToPdf/Program.vb)

### Convert a Presentation to Another Format

Converts a PPTX presentation to the specified format.

**Project folder**: *PowerPointPresentation* / *DocumentConversion* / *PresentationToPresentation*  
**Files to review**: [Program.cs](./CS/PowerPointPresentation/DocumentConversion/PresentationToPresentation/Program.cs) | [Program.vb](./VB/PowerPointPresentation/DocumentConversion/PresentationToPresentation/Program.vb)

### Add Speaker Notes to a Presentation

Adds text notes to each slide in a presentation.

**Project folder**: *PowerPointPresentation* / *DocumentElements* / *PresentationNotes*  
**Files to review**: [Program.cs](./CS/PowerPointPresentation/DocumentElements/PresentationNotes/Program.cs) | [Program.vb](./VB/PowerPointPresentation/DocumentElements/PresentationNotes/Program.vb)

### Add a Table to a Presentation Slide

Creates a presentation slide with a table.

**Project folder**: *PowerPointPresentation* / *DocumentElements* / *PresentationTables*  
**Files to review**: [Program.cs](./CS/PowerPointPresentation/DocumentElements/PresentationTables/Program.cs) | [Program.vb](./VB/PowerPointPresentation/DocumentElements/PresentationTables/Program.vb)

### Merge Presentations

Merges multiple presentations into a single presentation.

**Project folder**: *PowerPointPresentation* / *MergeSplit* / *PresentationMerge*  
**Files to review**: [Program.cs](./CS/PowerPointPresentation/MergeSplit/PresentationMerge/Program.cs) | [Program.vb](./VB/PowerPointPresentation/MergeSplit/PresentationMerge/Program.vb)

### Split a Presentation

Splits a presentation into multiple single-slide presentations in PPTX or PDF format.

**Project folder**: *PowerPointPresentation* / *MergeSplit* / *PresentationSplit*  
**Files to review**: [Program.cs](./CS/PowerPointPresentation/MergeSplit/PresentationSplit/Program.cs) | [Program.vb](./VB/PowerPointPresentation/MergeSplit/PresentationSplit/Program.vb)

### Extract Images from a Presentation

Extracts all images from slides and saves them as PNG files.

**Project folder**: *PowerPointPresentation* / *SearchExtract* / *PresentationExtractImages*  
**Files to review**: [Program.cs](./CS/PowerPointPresentation/SearchExtract/PresentationExtractImages/Program.cs) | [Program.vb](./VB/PowerPointPresentation/SearchExtract/PresentationExtractImages/Program.vb)

### Extract Text from a Presentation

Extracts text from slide shapes, tables, and notes. Generates a TXT file with extracted content. 

**Project folder**: *PowerPointPresentation* / *SearchExtract* / *PresentationExtractText*  
**Files to review**: [Program.cs](./CS/PowerPointPresentation/SearchExtract/PresentationExtractText/Program.cs) | [Program.vb](./VB/PowerPointPresentation/SearchExtract/PresentationExtractText/Program.vb)

### Find and Replace Text in a Presentation

Finds text in slide shapes and tables. Replaces, removes, or highlights found text.

**Project folder**: *PowerPointPresentation* / *SearchExtract* / *PresentationFindReplace*  
**Files to review**: [Program.cs](./CS/PowerPointPresentation/SearchExtract/PresentationFindReplace/Program.cs) | [Program.vb](./VB/PowerPointPresentation/SearchExtract/PresentationFindReplace/Program.vb)

### Extract Presentation Metadata

Extracts metadata (such as title, author, and creation date) from a presentation.

**Project folder**: *PowerPointPresentation* / *SearchExtract* / *PresentationMetadata*  
**Files to review**: [Program.cs](./CS/PowerPointPresentation/SearchExtract/PresentationMetadata/Program.cs) | [Program.vb](./VB/PowerPointPresentation/SearchExtract/PresentationMetadata/Program.vb)

## More Examples

- [Office File API Online Demos](https://demos.devexpress.com/OfficeFileAPI/BlazorServer/)
- [Word Processing Document API – How to Process Word Documents in Code](https://github.com/DevExpress-Examples/word-document-api-examples)
- [Spreadsheet Document API – How to Process Spreadsheet Documents in Code (Part 1)](https://github.com/DevExpress-Examples/spreadsheet-document-api-examples-part1)
- [Spreadsheet Document API – How to Process Spreadsheet Documents in Code (Part 2)](https://github.com/DevExpress-Examples/spreadsheet-document-api-examples-part2)
