export function getAttribute(object, attribute) { return object[attribute]; }

export async function arrayBuffer(buffer) {
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

export function registerEventHandlers(fileReader, jSInstance) {
    jSInstance.addEventListener('loadstart', (e) => fileReader.objRef.invokeMethodAsync('InvokeOnLoadStart', DotNet.createJSObjectReference(e)));
    jSInstance.addEventListener('progress', (e) => fileReader.objRef.invokeMethodAsync('InvokeOnProgress', DotNet.createJSObjectReference(e)));
    jSInstance.addEventListener('load', (e) => fileReader.objRef.invokeMethodAsync('InvokeOnLoad', DotNet.createJSObjectReference(e)));
    jSInstance.addEventListener('abort', (e) => fileReader.objRef.invokeMethodAsync('InvokeOnAbort', DotNet.createJSObjectReference(e)));
    jSInstance.addEventListener('error', (e) => fileReader.objRef.invokeMethodAsync('InvokeOnError', DotNet.createJSObjectReference(e)));
    jSInstance.addEventListener('loadend', (e) => fileReader.objRef.invokeMethodAsync('InvokeOnLoadEnd', DotNet.createJSObjectReference(e)));
}

export function isArrayBuffer(fileReader) {
    return (fileReader.result instanceof ArrayBuffer)
}