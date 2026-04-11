export function initCopyButtonClipboard() {
    const ClipboardJS = window.ClipboardJS;
    if (!ClipboardJS) return;

    window.copyButtonClipboard ??= new ClipboardJS(".copy-code-btn");
    window.copyButtonClipboard.on("success", e => {
        e.clearSelection();
    });
}
