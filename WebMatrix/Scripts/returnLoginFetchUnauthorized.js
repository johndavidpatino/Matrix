(function () {
	var originalFetch = fetch;
	fetch = function () {
		return originalFetch.apply(this, arguments).then(function (data) {
			if (data.status == 401)
				window.location.href = window.location.origin + "/ReturnUrl=" + window.location.pathname;
			else if (data.status != 200)
				alert('Ha ocurrido un error!')
			return data;
		});
	};
})();