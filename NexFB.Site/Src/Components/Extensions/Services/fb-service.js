'use strict';
/*global appModule*/

var fbGeneralService = function () {
	var memberNewOrder;
	var memberSavedOrder;
	var memberEditOrder;

	return {
		getMemberNewOrder: function () {
			return memberNewOrder;
		},
		setMemberNewOrder: function (value) {
			memberNewOrder = value;
		},
		getMemberSavedOrder: function () {
			return memberSavedOrder;
		},
		setMemberSavedOrder: function (value) {
			memberSavedOrder = value;
		},
		getMemberEditOrder: function () {
			return memberEditOrder;
		},
		setMemberEditOrder: function (value) {
			memberEditOrder = value;
		}
	};
};

appModule.service('orderingService', fbGeneralService);