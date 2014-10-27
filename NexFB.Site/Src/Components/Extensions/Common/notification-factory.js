/*global appModule, toastr*/
'use strict';

appModule.factory('notificationFactory', ['$translate', function ($translate) {
	var options = {
		timeout: 3000
	};

	return {
		success: function (keyMessage, message) {
			if (message === '' || message === null || message === undefined) {
				$translate(keyMessage).then(function (responseMessage) {
					toastr.success(responseMessage, options);
				});
			}
			else {
				toastr.success(message, options);
			}
		},
		error: function (keyMessage, message) {
			if (message === '' || message === null || message === undefined) {
				$translate(keyMessage).then(function (responseMessage) {
					toastr.error(responseMessage, options);
				});
			}
			else {
				toastr.error(message, options);
			}
		},
		warning: function (keyMessage, message) {
			if (message === '' || message === null || message === undefined) {
				$translate(keyMessage).then(function (responseMessage) {
					toastr.warning(responseMessage, options);
				});
			}
			else {
				toastr.warning(message, options);
			}
		},
		info: function (keyMessage, message) {
			if (message === '' || message === null || message === undefined) {
				$translate(keyMessage).then(function (responseMessage) {
					toastr.info(responseMessage, options);
				});
			}
			else {
				toastr.info(message, options);
			}
		}
	};
} ]);