function loadImageFromInputFileToTagImage(input,img) {
	let url = URL.createObjectURL(input.files[0]);
	img.src = url;
}