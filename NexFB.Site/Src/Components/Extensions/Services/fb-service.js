'use strict';
/*global appModule*/

var fbGeneralService = function () {
	var memberNewOrder;
	var memberSavedOrder;
	var memberEditOrder;
	var currentUser;
	var myOrder;
	var memberSideDishOrder;

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
		},
		getCurrentUser: function () {
			return currentUser;
		},
		setCurrentUser: function (value) {
			currentUser = value;
		},
		getMyOrder: function () {
			return myOrder;
		},
		setMyOrder: function (value) {
			myOrder = value;
		},
		getMemberSideDishOrder: function () {
			return memberSideDishOrder;
		},
		setMemberSideDishOrder: function (value) {
			memberSideDishOrder = value;
		}
	};
};

appModule.service('orderingService', fbGeneralService);