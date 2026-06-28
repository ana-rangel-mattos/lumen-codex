import { useQuery } from "@tanstack/react-query";
import { getCourses } from "../services/courseApi.ts";

export const useLoadCourses = () => {
  const { isPending, error, data } = useQuery({
    queryKey: ["courses"],
    queryFn: () => getCourses(),
  });

  return { isPending, error, data };
};
