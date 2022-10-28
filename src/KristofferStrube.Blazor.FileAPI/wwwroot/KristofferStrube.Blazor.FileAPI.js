export function getAttribute(object, attribute) { return object[attribute]; }

export async function arrayBuffer(blob) {
    var buffer = await blob.arrayBuffer();
    var bytes = new Uint8Array(buffer);
    return bytes;
}