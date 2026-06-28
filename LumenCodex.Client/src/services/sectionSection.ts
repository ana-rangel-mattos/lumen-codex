import { ApiClient } from "./apiClient.ts";
import { BASE_URL, URL_COURSES_API } from "../utils/constants.ts";
import type { Section } from "../types/ICourse.ts";

class SectionService extends ApiClient {
  constructor() {
    super(`${BASE_URL}${URL_COURSES_API}`);
  }

  getSections(courseId: string): Promise<Section[]> {
    return this.get(`${courseId}/sections`);
  }
}

export const sectionService = new SectionService();
