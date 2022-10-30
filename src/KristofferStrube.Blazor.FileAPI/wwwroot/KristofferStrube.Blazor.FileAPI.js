export function getAttribute(object, attribute) { return object[attribute]; }

export async function arrayBuffer(blob) {
    var buffer = await blob.arrayBuffer();
    var bytes = new Uint8Array(buffer);
    return bytes;
}

export function constructBlob(blobParts, options) {
    return new Blob(blobParts, options);
}

export function constructFile(blobParts, fileName, options) {
    return new File(blobParts, fileName, options);
}

export function constructFileReader() {
    return new FileReader();
}