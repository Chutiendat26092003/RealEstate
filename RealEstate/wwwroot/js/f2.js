var timer = undefined;
timer = setTimeout(() => {
	//<![CDATA[
	function loadCSS(e, t, n) {
		"use strict";
		var i = window.document.createElement("link");
		var o = t || window.document.getElementsByTagName("footer")[0];
		i.rel = "stylesheet";
		i.href = e;
		i.media = "only x";
		o.parentNode.insertBefore(i, o);
		setTimeout(function () {
			i.media = n || "all";
		});
	}
	loadCSS(
		"https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css"
	);
	//]]>
	timer = undefined;
}, 2000);
