// Logined
const passLogin = document.querySelector("#pass-login");
const passLoginRe = document.querySelector("#pass-login-2");
const erroPass = document.querySelector("#error-noti");
const submitLog = document.querySelector(".form-login-submit button");

if (passLogin && passLoginRe) {
	passLoginRe.addEventListener("blur", function () {
		if (passLogin.value !== passLoginRe.value) {
			submitLog.classList.add("disabled");
			erroPass.classList.add("active");
		} else {
			submitLog.classList.remove("disabled");
			erroPass.classList.remove("active");
		}
	});
}

// Deatail

const imgItem = document.querySelectorAll(".img-detail-item");
const siderImgMain = document.querySelector(".detail-img > img");

if (imgItem && siderImgMain) {
	imgItem.forEach((item) => {
		item.addEventListener("click", (e) => {
			imgItem.forEach((itm) => {
				itm.classList.remove("active");
			});
			e.target.classList.add("active");
			siderImgMain.src = e.target.src;
		});
	});
}

// Post
const listImgUp = document.querySelector("#listImgUp");
const boxUp = document.querySelector("#box-img-up");

function renderIngUpload(arr) {
	let listUpHtml = [];
	arr.forEach((item) => {
		let itemHmtl = `
		<div class="box-img-up-item">
			<img src="${item}" alt="">
			<div class="btn-click-close"><i class="fa fa-times" aria-hidden="true"></i></div>
		</div>`;
		listUpHtml.push(itemHmtl);
	});

	boxUp.innerHTML = listUpHtml.join(" ");
}

if (listImgUp) {
	listImgUp.onchange = (e) => {
		const files = e.target.files;
		const fileURLs = [];
		for (let i = 0; i < files.length; i++) {
			fileURLs.push(URL.createObjectURL(files[i]));
		}
		renderIngUpload(fileURLs);
	};
}

const postMenu = document.querySelector(".post-menu");
const postSection = document.querySelector(".post-section");

window.addEventListener("scroll", () => {
	if (window.pageYOffset >= 105) {
		postMenu.classList.add("active");
	} else {
		postMenu.classList.remove("active");
	}
	if (window.pageYOffset > postSection.clientHeight - window.outerHeight) {
		postMenu.classList.remove("active");
		postMenu.classList.add("active2");
	} else {
		postMenu.classList.remove("active2");
	}
});

const postDay = document.querySelectorAll(".post-day-box input");
const postType = document.querySelectorAll(".post-type-box input");
const postPayBox = document.querySelector(".post-total-payment");
const postTotalBack = document.querySelector("#post-total-back");
const postBalance = document.querySelector("#postBalance");
/*const postBanlanceSpan = document.querySelector("#postBanlanceSpan");*/
let dayValue = 10;
let typeValue = "Tin thường";
let priceValue = 2182;
const listType = {
	thuong: "Tin thường",
	bac: "VIP Bạc",
	vang: "VIP Vàng",
	kimCuong: "VIP Kim Cương",
};
const listTypePrice = {
	thuong: 2182,
	bac: 49090,
	vang: 104545,
	kimCuong: 268184,
};

function calcTotalPost(day, type, price) {
	let postTotalHtml = `<h3>Thanh toán</h3>
	<div class="post-pay">
		<div>
			<p>Loại tin</p>
			<span>${type}</span>
		</div>
		<div>
			<p>Đơn giá / ngày</p>
			<span>${price.toLocaleString("vi-VN", {
				style: "currency",
				currency: "VND",
			})}</span>
		</div>
		<div>
			<p>Thời gian đăng tin</p>
			<span>${day} ngày</span>
		</div>
	</div>
	<div class="post-sum-btn">
		<p>Tổng tiền</p>
		<h3>${(day * price).toLocaleString("vi-VN", {
			style: "currency",
			currency: "VND",
		})}</h3>
	</div>
	<div class="post-sum-btn re">
		<div>
			<span id="postFormCheckNoti">Số dư không đủ vui lòng nạp thêm !</span>
		</div>	
		<div class="flex-cen">
			<button type="submit" id="postBtnSub">Thanh toán và đăng tin <span><i class="fa fa-chevron-right"
			aria-hidden="true"></i></span></button>
		</div>
	</div>`;
	postPayBox.innerHTML = postTotalHtml;
	postTotalBack.value = day * price;
	/*postBanlanceSpan.textContent = new Number(postBalance.value);*/

	let postFormCheckNoti = document.querySelector("#postFormCheckNoti");
	let postBtnSub = document.querySelector("#postBtnSub");
	if (new Number(postTotalBack.value) > new Number(postBalance.value)) {
		postFormCheckNoti.classList.add("active");
		postBtnSub.classList.add("active");
		postBtnSub.onclick = (e) => {
			e.preventDefault();
		};
	}
}

if (postDay) {
	postDay.forEach((item) => {
		item.addEventListener("change", (e) => {
			dayValue = new Number(e.target.value);
			calcTotalPost(dayValue, typeValue, priceValue);
		});
	});
}

if (postType) {
	postType.forEach((item) => {
		item.addEventListener("change", (e) => {
			typeValue = listType[e.target.value];
			priceValue = listTypePrice[e.target.value];
			calcTotalPost(dayValue, typeValue, priceValue);
		});
	});
}

calcTotalPost(dayValue, typeValue, priceValue);
