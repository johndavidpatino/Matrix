function imageToTextBase64FromInputFile(inputFile) {
	const temporaryFileReader = new FileReader();

	return new Promise((resolve, reject) => {
		temporaryFileReader.onerror = () => {
			temporaryFileReader.abort();
			reject(new DOMException("Problem parsing input file."));
		};

		temporaryFileReader.onload = () => {
			resolve(temporaryFileReader.result.replace('data:image/png;base64,', '').replace('data:image/jpeg;base64,', ''));
		};
		temporaryFileReader.readAsDataURL(inputFile);
	});
}