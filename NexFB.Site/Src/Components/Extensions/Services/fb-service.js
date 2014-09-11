'use strict';

var fbGeneralService = function () {
	var memberNewOrder;
	var memberSavedOrder;

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
		}
	};
};

appModule.service('orderingService', fbGeneralService);