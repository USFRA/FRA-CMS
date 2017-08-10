$(document).ready(function () {
    //global vars
    var form = $("#commentForm");
    var name = $("#name");
    var nameInfo = $("#requireName");
    var email = $("#email");
    var emailInfo = $("#requireEmail");
    var emailRegex = $("#regEmail");
    var comment = $("#comment");
    var commentInfo = $("#requireComment");

    function validateName() {
        //if it's NOT valid
        if (name.val().length < 1) {
            name.addClass("error");
            nameInfo.show();
            nameInfo.addClass("error");
            return false;
        }
        //if it's valid
        else {
            name.removeClass("error");
            nameInfo.hide();
            nameInfo.removeClass("error");
            return true;
        }
    }
    function validateEmail() {
        var pattern = new RegExp(/^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i);

        //if it's NOT valid
        if (email.val().length < 1) {
            email.addClass("error");
            emailInfo.show();
            emailInfo.addClass("error");
            return false;
        }
        else if (!pattern.test(email.val())) {
            emailInfo.hide();
            email.addClass("error");
            emailRegex.show();
            emailRegex.addClass("error");
            return false;
        }
        //if it's valid
        else {
            email.removeClass("error");
            emailInfo.hide();
            emailRegex.hide();
            emailInfo.removeClass("error");
            return true;
        }
    }
    function validateComment() {
        //it's NOT valid
        if (comment.val().length < 1) {
            comment.addClass("error");
            commentInfo.show();
            commentInfo.addClass("error");
            return false;
        }
        //it's valid
        else {
            comment.removeClass("error");
            commentInfo.hide();
            commentInfo.removeClass("error");
            return true;
        }
    }
//    function wireValidateEmail() {
//        email.blur(validateEmail);
//    }
    //On blur
    //name.blur(validateName);
    //email.blur(validateEmail);
    //comment.blur(validateComment);
    //On key press
    //name.keyup(validateName);
    //email.keyup(wireValidateEmail);
    //comment.keyup(validateComment);
    //On Submitting
    form.submit(function () {
        var v1 = validateName();
        var v2 = validateEmail();
        var v3 = validateComment();

        if (v1 && v2 && v3)
            return true;
        else
            return false;
    });
});