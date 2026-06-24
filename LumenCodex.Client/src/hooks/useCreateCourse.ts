import { useMutation, useQueryClient } from "@tanstack/react-query";
import { toast } from "react-toastify";
import type { IPostCourseRequest } from "../types/ICourse.ts";
import { coursesService } from "../services/coursesService.ts";

export const useCreateCourse = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (obj: IPostCourseRequest) => coursesService.uploadCourse(obj),
    onMutate: () => {
      const toastId = toast.loading("Creating course. Please wait...");
      return { toastId };
    },
    onSuccess: (_data, _variables, context) => {
      queryClient.invalidateQueries({ queryKey: ["courses"] });

      if (context?.toastId) {
        toast.update(context.toastId, {
          render: "Course was successfully added!",
          type: "success",
          isLoading: false,
          autoClose: 3000,
        });
      }
    },
    onError: (error: any, _variables, context) => {
      console.error("error creating course: ", error);
      const errorMessage = error?.response?.data || "Course path does not exist.";

      if (context?.toastId) {
        toast.update(context.toastId, { render: `Error: ${errorMessage}`, type: "error", isLoading: false, autoClose: 4000 })
      } else {
        toast.error(`Error: ${errorMessage}`);
      }
    },
  });
};