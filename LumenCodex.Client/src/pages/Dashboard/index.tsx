import { ErrorComponent } from "@tanstack/react-router";
import { useQuery } from "@tanstack/react-query";
import { getCourses } from "../../services/courseApi.ts";
import { CourseCard, Spinner } from "../../components";

const Dashboard = () => {
  const { isPending, error, data } = useQuery({
    queryKey: ["courses"],
    queryFn: () => getCourses(),
  });

  if (isPending) {
    return <Spinner />;
  }

  if (error) {
    return <ErrorComponent error={error} />;
  }

  return (
    <div className="mt-6 px-8 pb-6">
      <h2 className="text-base-100 text-4xl">
        Courses Dashboard{" "}
        <span className="text-base-300 text-xl">
          - Total courses {data?.meta.totalItems}
        </span>
      </h2>

      <ul className="mt-8 grid grid-cols-[repeat(auto-fill,minmax(24rem,1fr))] items-end gap-6">
        {data?.data?.map((course) => (
          <CourseCard key={course.courseId} course={course} />
        ))}
      </ul>

      <div className="join">
        <button className="join-item btn">«</button>
        <button className="join-item btn">Page 22</button>
        <button className="join-item btn">»</button>
      </div>
    </div>
  );
};

export default Dashboard;
