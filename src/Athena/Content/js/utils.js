export function focusInput(selector) {
    $(selector).focus();
    $(selector + ':text:visible:first').focus();
}