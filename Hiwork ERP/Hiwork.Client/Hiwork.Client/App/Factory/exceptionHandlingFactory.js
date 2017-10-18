/**
 * Created by Mahfuz on 07/24/2017.
 */

angular.module("hiworkApp").factory('exceptionHandler', ['alerting', function (alerting) {

    var service = {
        handleException: handleException
    };

    var msgText = [];

    return service;

    function handleException(error) {

        switch (error.status) {
            case 400:   // 'Bad Request'
                // Model state errors
                var errors = error.data.modelState;

                // Loop through and get all 
                // validation errors
                for (var key in errors) {
                    for (var i = 0; i < errors[key].length; i++) {
                        msgText.push({
                            message: errors[key][i]
                        });
                    }
                }
                alerting.addDanger(msgText.message);
                break;

            case 404:  // 'Not Found'
                msgText.push(
                  {
                      message: "The data you were " +
                                "requesting could not be found"
                  });
                alerting.addDanger(msgText[0].message);
                break;

            case 500:  // 'Internal Error'
                msgText.push(
                  {
                      message: error.data.ExceptionMessage
                  });

                alerting.addDanger(msgText[0].message);
                break;

            default:
                msgText.push(
                  {
                      message: "Status: " +
                              error.status +
                              " - Error Message: " +
                              error.statusText
                  });

                alerting.addDanger(msgText[0].message);
                break;
        }
    }

}]);