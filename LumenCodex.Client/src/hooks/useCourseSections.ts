import { useQuery } from "@tanstack/react-query";
import { sectionService } from "../services/sectionSection.ts";

export const useCourseSections = (courseId: string) => {
  const {
    isPending: isSectionLoading,
    error: sectionError,
    data: sectionsData,
  } = useQuery({
    queryKey: ["sections", courseId],
    queryFn: () => sectionService.getSections(courseId),
  });

  return { isSectionLoading, sectionError, sectionsData };
};