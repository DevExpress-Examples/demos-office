export async function ensureNodeVisible(selectedNodeId) {
    var selectedNode = document.getElementById(selectedNodeId);
    if (selectedNode) {
        var rect = selectedNode.getBoundingClientRect();
        if (rect.bottom < 0 || rect.top > window.innerHeight)
            selectedNode.scrollIntoView();
    }
}
