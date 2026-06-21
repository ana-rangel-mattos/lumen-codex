import { type FormEvent, useState } from "react";
import { Button } from "../../components";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { uploadCourse } from "../../services/courseApi.ts";
import toast from "react-hot-toast";

interface IUploadCourseProps {
  path: string;
  isSingleCourse: boolean;
}

const HomePage = () => {
  const [path, setPath] = useState("");
  const [isSingleCourse, setIsSingleCourse] = useState(true);
  const queryClient = useQueryClient();

  const handleSubmit = (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    console.log(path);

    if (!path) {
      toast.error("Root path is required!");
      return;
    }

    courseMutation.mutate({ path, isSingleCourse });
  };

  const courseMutation = useMutation({
    mutationFn: (obj: IUploadCourseProps) =>
      uploadCourse(obj.path, obj.isSingleCourse),
    onSuccess: async () => {
      toast.success("Course was added successfully!");
      setPath("");
      await queryClient.invalidateQueries({ queryKey: ["courses"] });
    },
    onError: (error) => {
      toast.error(error.message);
    },
  });

  if (courseMutation.isPending) {
    toast.loading("Uploading course it could take a while...");
  }

  return (
    <div className="x-8 mt-6 flex h-full flex-col items-center">
      <h2 className="text-base-100 text-4xl">Add courses to your library</h2>

      <form className="mt-54 flex h-full flex-col" onSubmit={handleSubmit}>
        <label className="input text-lg">
          Root Path:
          <input
            onChange={(e) => setPath(e.target.value)}
            value={path}
            type="text"
            className="grow"
            placeholder="C://user..."
          />
          <span className="badge badge-error badge-sm">Required</span>
        </label>

        <label className="input my-4 text-lg">
          Single Course:
          <input
            type="checkbox"
            checked={isSingleCourse}
            onChange={(e) => setIsSingleCourse(e.target.checked)}
            className="checkbox checkbox-success"
          />
        </label>

        <Button type="submit">Submit</Button>
      </form>
    </div>
  );
};

export default HomePage;
