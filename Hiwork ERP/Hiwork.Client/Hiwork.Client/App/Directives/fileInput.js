


angular.module('hiworkApp').directive('customOnChange', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var onChangeFunc = scope.$eval(attrs.customOnChange);
            element.bind('change', onChangeFunc);
        }
    };
});


angular.module("hiworkApp").directive('loadImage', function () {
    return {
        require: "ngModel",
        restrict: 'A',
        link: function ($scope, el, attrs, ngModel) {
            el.bind('change', function (event) {
                var files = event.target.files;
                var file = files[0];

                var reader = new FileReader();
                reader.onload = function (evt) {
                    ngModel.$setViewValue(null);
                    ngModel.$setViewValue(evt.target.result);       // Set model data to base64 encoded string of selected file
                    $scope.$apply();
                };
                reader.readAsDataURL(file);
            });
        }
    };
});


angular.module("hiworkApp").directive('loadFile', function () {
    return {
        require: "ngModel",
        restrict: 'A',
        link: function ($scope, el, attrs, ngModel) {
            el.bind('change', function (event) {
                var isMultiple = attrs.multiple;
                var FileList = event.target.files;
                var File = FileList[0];

                if (isMultiple == true) {
                    var i;
                    var values = new Array(FileList.length);
                    for (i = 0; i < FileList.length; i++) {
                        values[i] = FileList.item(i);
                    }
                    ngModel.$setViewValue(values);                // Set model data to the file selected
                }
                else {
                    ngModel.$setViewValue(File);                // Set model data to the file selected
                }
                $scope.$apply();
            });
        }
    };
});


