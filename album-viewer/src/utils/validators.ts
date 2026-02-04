// function named `validateDate` which validates a date from text input in a french format and converts it to a date object.
export function validateDate(dateString: string): Date | null {
  // Regular expression to match French date format (DD/MM/YYYY)
  const frenchDateRegex = /^(\d{2})\/(\d{2})\/(\d{4})$/;
  const match = dateString.match(frenchDateRegex);
  // If the input doesn't match the French date format, return null
  if (!match) {
    return null;
  }
  const day = parseInt(match[1]!, 10);
  const month = parseInt(match[2]!, 10) - 1; // Months are zero-based in JS Date
  const year = parseInt(match[3]!, 10);
  const date = new Date(year, month, day);
  // Check if the constructed date is valid
  if (
    date.getFullYear() === year &&
    date.getMonth() === month &&
    date.getDate() === day
  ) {
    return date;
  }
  return null;
}

//validateIPV6 function which validates if a given string is a valid IPV6 address.
export function validateIPV6(ipv6String: string): boolean {
  // Regular expression to match valid IPV6 addresses
  const ipv6Regex = /^(([0-9a-fA-F]{1,4}:){7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))$/;
  return ipv6Regex.test(ipv6String);
}