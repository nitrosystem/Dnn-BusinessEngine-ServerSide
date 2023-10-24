var bField_SimpleCaptcha = function ($scope, field) {
    var code;

    this.init = function () {
        createCaptcha();

        if (!field.IsShow) {
            $scope.$watch('Field.' + field.FieldName + '.IsShow', function (newVal, oldVal) {
                if (newVal != oldVal && newVal) {
                    setTimeout(function () {
                        createCaptcha(field);
                    }, 1000);
                }
            });
        }
    }

    function createCaptcha(field) {
        //clear the contents of captcha div first 
        $('#bCaptcha_' + field.FieldName).html('');
        var charsArray =
            "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var lengthOtp = 6;
        var captcha = [];
        for (var i = 0; i < lengthOtp; i++) {
            //below code will not allow Repetition of Characters
            var index = Math.floor(Math.random() * charsArray.length + 1); //get the next character from the array
            if (captcha.indexOf(charsArray[index]) == -1)
                captcha.push(charsArray[index]);
            else i--;
        }
        var canv = document.createElement("canvas");
        canv.id = "captcha";
        canv.width = 100;
        canv.height = 50;
        var ctx = canv.getContext("2d");
        ctx.font = "25px Georgia";
        ctx.strokeText(captcha.join(""), 0, 30);
        //storing captcha so that can validate you can save it somewhere else according to your specific requirements
        code = captcha.join("");
        $('#bCaptcha_' + field.FieldName).append(canv); // adds the canvas to the body element
    }

    $scope.bSimpleCaptcha_validateCaptcha = function (field) {
        if ($('#bCaptchaInput_' + field.FieldName).val() == code) {
            return true;
        } else {
            createCaptcha(field);

            return false;
        }
    }
}