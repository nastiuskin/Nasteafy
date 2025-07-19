import { Filter } from "../api/apiClient";

export function buildFilter(path: string, value: string) {
  const f = new Filter();
  f.init({ path, value });
  return f;
}