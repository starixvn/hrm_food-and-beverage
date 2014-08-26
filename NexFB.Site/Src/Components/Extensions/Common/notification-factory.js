/*global appModule, toastr*/
'use strict';

appModule.factory('notificationFactory', ['$translate', function ($translate) {
	return {
		success: function (keyMessage, message) {
			if (message === '' || message === null || message === undefined) {
				$translate(keyMessage).then(function (responseMessage) {
					toastr.success(responseMessage);
				});
			}
			else {
				toastr.success(message);
			}
		},
		error: function (keyMessage, message) {
			if (message === '' || message === null || message === undefined) {
				$translate(keyMessage).then(function (responseMessage) {
					toastr.error(responseMessage);
				});
			}
			else {
				toastr.error(message);
			}
		},
		warning: function (keyMessage, message) {
			if (message === '' || message === null || message === undefined) {
				$translate(keyMessage).then(function (responseMessage) {
					toastr.warning(responseMessage);
				});
			}
			else {
				toastr.warning(message);
			}
		},
		info: function (keyMessage, message) {
			if (message === '' || message === null || message === undefined) {
				$translate(keyMessage).then(function (responseMessage) {
					toastr.info(responseMessage);
				});
			}
			else {
				toastr.info(message);
			}
		}
	};
} ]);