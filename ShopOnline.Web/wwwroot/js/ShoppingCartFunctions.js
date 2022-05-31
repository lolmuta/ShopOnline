function MakeUpdateQtyButtonVisible(id, visible)
{
    const updateQtnBtn = document.querySelector("button[data-itemId='" + id + "']");
    if (visible) {
        updateQtnBtn.style.display = "inline-block";
    } else {
        updateQtnBtn.style.display = "none";
    }
}