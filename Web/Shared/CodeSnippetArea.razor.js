export function highlightAllUnder(root) {
    if (!window.hljs) return
    if (!root) return;
    const nodes = (root).querySelectorAll("pre code");
    nodes.forEach(el => window.hljs.highlightElement(el));
}
