import { describe, expect, it } from "vitest";
import { validateDate, validateIPV6 } from "./validators";

describe("validateDate", () => {
  it("should parse valid French date format (DD/MM/YYYY)", () => {
    const result = validateDate("25/12/2023");
    expect(result).toEqual(new Date(2023, 11, 25));
  });

  it("should return null for invalid format", () => {
    expect(validateDate("2023-12-25")).toBeNull();
    expect(validateDate("12/25/2023")).toBeNull();
    expect(validateDate("25-12-2023")).toBeNull();
  });

  it("should return null for invalid day", () => {
    expect(validateDate("32/01/2023")).toBeNull();
    expect(validateDate("31/02/2023")).toBeNull();
  });

  it("should return null for invalid month", () => {
    expect(validateDate("15/13/2023")).toBeNull();
    expect(validateDate("15/00/2023")).toBeNull();
  });

  it("should handle leap year dates correctly", () => {
    const leapYearDate = validateDate("29/02/2020");
    expect(leapYearDate).toEqual(new Date(2020, 1, 29));

    const nonLeapYearDate = validateDate("29/02/2021");
    expect(nonLeapYearDate).toBeNull();
  });

  it("should return null for empty or malformed strings", () => {
    expect(validateDate("")).toBeNull();
    expect(validateDate("25/12")).toBeNull();
    expect(validateDate("abc/def/ghij")).toBeNull();
  });

  it("should handle single digit padding", () => {
    expect(validateDate("01/01/2023")).toEqual(new Date(2023, 0, 1));
  });
});

//validateIPV6 function which validates if a given string is a valid IPV6 address.
describe("validateIPV6", () => {
  it("should return true for valid IPV6 addresses", () => {
    expect(validateIPV6("2001:0db8:85a3:0000:0000:8a2e:0370:7334")).toBe(true);
    expect(validateIPV6("2001:db8:85a3:0:0:8a2e:370:7334")).toBe(true);
    expect(validateIPV6("2001:db8:85a3::8a2e:370:7334")).toBe(true);
    expect(validateIPV6("::1")).toBe(true);
    expect(validateIPV6("fe80::1ff:fe23:4567:890a")).toBe(true);
    expect(validateIPV6("::ffff:192.168.1.1")).toBe(true);
  });

  it("should return false for invalid IPV6 addresses", () => {
    expect(validateIPV6("2001:0db8:85a3:0000:0000:8a2e:0370:7334:1234")).toBe(false); // Too many segments
    expect(validateIPV6("2001:db8:85a3:0:0:8a2e:370g:7334")).toBe(false); // Invalid character 'g'
    expect(validateIPV6("2001:db8:85a3::8a2e::370:7334")).toBe(false); // Multiple '::'
    expect(validateIPV6("12345::1")).toBe(false); // Segment too long
    expect(validateIPV6("::ffff:192.168.1.256")).toBe(false); // Invalid IPv4 part
    expect(validateIPV6("just:a:string")).toBe(false); // Not an IPV6 address
  });
});