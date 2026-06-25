import { type FormEvent, useState } from "react";
import { Button } from "../../components";
import { useCreateCourse } from "../../hooks/useCreateCourse.ts";
import { toast } from "react-toastify";

const HomePage = () => {
  const [path, setPath] = useState("");
  const [isSingleCourse, setIsSingleCourse] = useState(true);
  const createCourse = useCreateCourse();

  const handleSubmit = (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    if (!path) {
      toast.error("Course path is required!");
      return;
    }

    createCourse.mutate({ path, isSingleCourse });
  };

  return (
    <div className="x-8 mt-6 flex h-full flex-col items-center">
      <h2 className="text-base-100 text-4xl">Add courses to your library</h2>

      <form
        className="mt-30 flex h-full w-full flex-col items-center gap-y-4"
        onSubmit={handleSubmit}
      >
        <label className="input mx-auto text-lg sm:w-3/4 md:w-4/6 lg:w-2/4">
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

        <label className="input mx-auto my-4 text-lg sm:w-3/4 md:w-4/6 lg:w-2/4">
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
