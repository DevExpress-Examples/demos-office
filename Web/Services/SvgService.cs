using Microsoft.AspNetCore.Components;

namespace BlazorDemo.OfficeFileApi {
    interface IOfficeFileApiBlazorDemoSectionSvgService {
        public MarkupString DocumentConversion { get; }
        public MarkupString MailMergeDocumentTemplates { get; }
        public MarkupString PdfInteractiveForms { get; }
        public MarkupString SecurityDocumentProtection { get; }
        public MarkupString SplitMergeDocuments { get; }
        public MarkupString DocumentContentManipulation { get; }
    }

    class SvgService : IOfficeFileApiBlazorDemoSectionSvgService {
        public MarkupString DocumentConversion => new(
            """
            <svg id="svg-DocumentConversion" viewBox="0 0 84 84">
                <g>
                    <path class="st0" d="M44.12,62c0.73-5.09,5.09-9,10.38-9c3.67,0,6.89,1.88,8.77,4.73l2.86-2.86C63.49,51.31,59.27,49,54.5,49c-7.5,0-13.65,5.7-14.41,13H36l6,5.9l6-5.9H44.12z" />
                    <path class="st0" d="M67,59l-6,6h3.88c-0.73,5.09-5.09,9-10.38,9c-3.67,0-6.9-1.89-8.77-4.74l-2.86,2.86C45.51,75.68,49.73,78,54.5,78c7.5,0,13.65-5.7,14.4-13H73L67,59z" />
                </g>
                <path class="st0" d="M46,5H10v67h30.01l-3.98-4H14V9h28v18h18v18.6c1.42,0.63,2.74,1.44,3.95,2.39L64,47.94V23L46,5z M46,23V11l12,12H46z" />
            </svg>
            """);

        public MarkupString MailMergeDocumentTemplates => new(
            """
            <svg id="svg-MailMerge" viewBox="0 0 84 84">
                <path class="st0" d="M64,36h-4h-8v-9v-4L34,5h-4H5v59h19v16h58V58v-4L64,36z M64,42l12,12H64V42z M46,23H34V11L46,23z M9,60V9h21v18h18v9H24v24H9z M48,40v20H28V40H48z M78,76H28V64h24V40h8v18h18V76z" />
            </svg>
            """);

        public MarkupString PdfInteractiveForms => new(
            """
            <svg id="svg-PdfForms" viewBox="0 0 84 84">
                <g>
                    <path class="st0" d="M50,34H14v11h36V34z M46,41H18v-3h28V41z" />
                    <path class="st0" d="M14,61h36V50H14V61z M18,54h28v3H18V54z" />
                    <polygon class="st0" points="43,84 50,84 43,77 	" />
                    <path class="st0" d="M71,50L45,76l7,6l26-26L71,50z M49,76l-1-1l23-23l1,1L49,76z" />
                    <path class="st0" d="M29.8,68H24H9V9h28v18h18v9v8.6V57l4-4v-6.1V36V23L41,5H5v67h19h7.7H41l4-4H29.8z M41,11l12,12H41V11z" />
                </g>
            </svg>
            """);

        public MarkupString SecurityDocumentProtection => new(
            """
            <svg id="svg-DocumentProtection" viewBox="0 0 84 84">
                <g>
                    <path class="st0" d="M71.58,51.56c-1.14-5.16-5.75-9.53-10.96-10.39c-5.02-0.83-10.07,1.43-12.46,5.66c-1.84,3.25-1.99,6.69-0.87,10.22c0.05,0.15,0.1,0.29,0.16,0.5c-0.72,0.08-1.39,0.16-2.06,0.24c-0.41,0.05-0.83,0.06-1.23,0.14c-0.98,0.2-1.29,1.16-0.62,1.9c0.53,0.59,0.98,1.1,1.63,1.81c-0.18,0.18-0.28,0.28-0.41,0.41c-5.13,5.12-10.26,10.25-15.4,15.37c-0.33,0.33-0.59,0.69-0.71,1.14c-0.19,0.76-0.65,2.43-0.6,3.12c0.08,0.26,0.14,0.52,0.23,0.77c0.17,0.5,0.73,0.64,1.14,0.3c0.16-0.14,0.31-0.3,0.46-0.45c7.15-7.06,14.29-14.11,21.44-21.17c0.15-0.14,0.29-0.29,0.44-0.43c0.03,0.02-1.85,3.53-2.66,4.32c-5.9,5.77-11.78,11.58-17.66,17.37c-0.1,0.1-0.22,0.18-0.34,0.26c0.02,0.03,0.03,0.07,0.05,0.1c0.37,0,0.74,0.01,1.12,0c0.84-0.02,1.69-0.03,2.53-0.08c0.19-0.01,0.41-0.1,0.56-0.22c0.36-0.29,1.84-1.86,1.84-2.58c0-0.87-0.31-1.83,0.5-1.83c1.13,0,2.26,0,3.38-0.05c0.24-0.01,0.5-0.14,0.7-0.29c0.95-0.67,1.34-1.56,1.21-2.74c-0.09-0.77-0.02-1.56-0.02-2.39c1.54,0,2.99,0,4.49,0c0.01-0.19,0.08-2.2,0.11-3.06c0-0.12,0.02-0.28,0.09-0.36c0.77-0.84,1.56-1.67,2.28-2.43c0.76,0.72,2.6,2.47,2.83,2.6c0.57,0.34,1.04,0.15,1.22-0.49c0.3-1.07,0.57-2.15,0.86-3.22c0.03-0.11,0.07-0.21,0.11-0.34c0.63,0.19,1.23,0.41,1.84,0.55c4.43,1,8.44,0.24,11.72-3.07c2-2.02,3.05-4.51,3.31-7.34c0.01-0.14,0.06-0.28,0.09-0.41c0-0.46,0-0.91,0-1.37C71.82,52.95,71.73,52.25,71.58,51.56z M62.9,52.27c-1.58,0.01-2.88-1.28-2.89-2.84c0-1.6,1.24-2.83,2.86-2.83c1.53,0,2.83,1.32,2.83,2.85C65.7,50.99,64.44,52.26,62.9,52.27z" />
                    <path class="st0" d="M31,71l4-4h-1.17H29H14V8h28v18h18v5v6.09c0.21,0.02,0.42,0.04,0.62,0.08c1.17,0.19,2.3,0.57,3.38,1.09V31v-9L46,4H10v67h19H31z M46,10l12,12H46V10z" />
                </g>
            </svg>
            """);

        public MarkupString SplitMergeDocuments => new(
            """
            <svg id="svg-SplitMerge" viewBox="0 0 84 84">
                <g>
                    <path class="st0" d="M62,2H33v30H8v50h40h4V52h21h4V18L62,2z M62,8l9,10h-9V8z M37,38l9,10h-9V38z M48,78H12V36l21,0.14V52h15V78zM73,48H52L37,32V6h21v16h15V48z" />
                    <polygon class="st0" points="32,56 28,56 28,63 21,63 21,67 28,67 28,74 32,74 32,67 39,67 39,63 32,63 	" />
                    <rect x="47" y="32" class="st0" width="17" height="4" />
                </g>
            </svg>
            """);

        public MarkupString DocumentContentManipulation => new(
            """
            <svg id="svg-ContentManipulation" viewBox="0 0 84 84">
                <g>
                    <path class="st0" d="M56.58,51c-0.77-1.76-2.53-3-4.58-3s-3.81,1.24-4.58,3H42v4h5.42c0.77,1.76,2.53,3,4.58,3s3.81-1.24,4.58-3H77v-4H56.58z" />
                    <path class="st0" d="M52,68c-2.05,0-3.81,1.24-4.58,3H42v4h5.42c0.77,1.76,2.53,3,4.58,3s3.81-1.24,4.58-3H77v-4H56.58C55.81,69.24,54.05,68,52,68z" />
                    <path class="st0" d="M66,58c-2.05,0-3.81,1.24-4.58,3H42v4h19.42c0.77,1.76,2.53,3,4.58,3s3.81-1.24,4.58-3H77v-4h-6.42C69.81,59.24,68.05,58,66,58z" />
                    <rect x="14" y="47" class="st0" width="23" height="4" />
                    <rect x="14" y="39" class="st0" width="33" height="4" />
                    <rect x="14" y="31" class="st0" width="33" height="4" />
                    <rect x="14" y="23" class="st0" width="33" height="4" />
                    <rect x="14" y="15" class="st0" width="33" height="4" />
                    <polygon class="st0" points="9,9 52,9 52,43 56,43 56,9 56,5 52,5 5,5 5,65 37,65 37,61 9,61 	" />
                </g>
            </svg>
            """);
    }
}
