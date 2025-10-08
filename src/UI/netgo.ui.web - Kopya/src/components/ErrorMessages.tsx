type ErrorMessagesProps = {
  errors: { [key: string]: string[] } | null | undefined;
};

export default function ErrorMessages({ errors }: ErrorMessagesProps) {
  if (!errors) return null;

  return (
    <div className="text-red-500 space-y-1">
      {Object.entries(errors).map(([field, messages]) => (
        <div key={field}>
          <strong>{field}:</strong>
          <ul>
            {messages.map((msg, idx) => (
              <li key={idx}>{msg}</li>
            ))}
          </ul>
        </div>
      ))}
    </div>
  );
}
