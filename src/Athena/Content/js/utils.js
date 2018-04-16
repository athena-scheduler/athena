export function focusInput(selector) {
    $(selector).focus();
    $(selector + ':text:visible:first').focus();
}

export function* chunk(arr, chunkSize) {
    for (let i = 0, j = arr.length; i < j; i += chunkSize ) {
        yield arr.slice(i,  i + chunkSize)
    }
}