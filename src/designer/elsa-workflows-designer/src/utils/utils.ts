import moment from "moment";

export function formatTimestamp(timestamp?: Date, defaultText?: string): string {
  return !!timestamp ? moment(timestamp).format('DD-MM-YYYY HH:mm:ss') : defaultText;
}

export function parseJson(json: string): any {
  if (!json)
    return null;

  try {
    return JSON.parse(json);
  } catch (e) {
    console.warn(`Error parsing JSON: ${e}`);
  }
  return undefined;
}
