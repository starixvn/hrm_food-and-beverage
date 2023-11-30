$(document).ready(function () {
    $('form', '#formContainer').submit(function () {
        let $password = $('#Password');
        $password.val(btoa($password.val()));
    });
});