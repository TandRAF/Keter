import { useState,useRef,useEffect } from "react";
import styles from "./InputData.module.scss";
import { Button } from "../Button/Button";
import { ShowPassword,HidePassword, Calendar, DropDown} from "../Icons/Icons";
import { span } from "framer-motion/client";

type UserOption = {
  id: string;
  userName: string;
  profilePictureUrl?: string;
};

type InputDataProps = {
  type:
    | "text"
    | "email"
    | "password"
    | "phone"
    | "calendar"
    | "select"
    | "user-select"
    | "radio"
    | "textarea";
  id: string;
  label: string;
  placeholder?: string;
  required?: boolean;
  options?: string[];
  userOptions?: UserOption[];
  onChange?: (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => void;
};

const InputData = ({
  type,
  id,
  label,
  placeholder,
  required = false,
  options = [],
  userOptions = [],
  onChange,
}: InputDataProps) => {
  const [focused, setFocused] = useState(false);
  const [isValid, setIsValid] = useState(true);
  const [showPassword, setShowPassword] = useState(false);

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    setIsValid(e.target.validity.valid);
    onChange?.(e);
  };

  switch (type) {
    case "email":
      return (
        <div className={styles.formInput}>
          <label htmlFor={id}>{label}</label>
          <input
            className={`${styles.email} ${isValid ? "" : styles.error}`}
            id={id}
            type="email"
            placeholder={placeholder ?? "Enter your email"}
            required={required}
            onChange={handleChange}
            onBlur={() => setFocused(true)}
            data-focused={focused.toString()}
            pattern="^[\w.\-]+@([\w\-]+\.)+[\w\-]{2,4}$"
          />
          {focused && !isValid && (
            <span className="error">Please enter a valid email address</span>
          )}
        </div>
      );

    case "password":
      return (
        <div
        className={styles.formInput}>
          <label htmlFor={id}>{label}</label>
          <div 
            className={`${styles.password} ${isValid ? "" : styles.error}`}>
            <input
              id={id}
              type={showPassword ? "text" : "password"}
              placeholder={placeholder ?? "Enter your password"}
              required={required}
              onChange={handleChange}
              onBlur={() => setFocused(true)}
              data-focused={focused.toString()}
              pattern="^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$"
            />
            <div className={styles['line']}></div>
            <Button
              size="sm"
              type="button"
              className={styles['TogglePassword']}
              icon={showPassword ? <ShowPassword/> : <HidePassword/>}
              onClick={() => setShowPassword((prev) => !prev)}
            >
            </Button>
          </div>
          {focused && !isValid && (
            <span className="error">
              Password must be at least 8 characters, include letters, numbers,
              and a special character (@$!%*?&)
            </span>
          )}
        </div>
      );

  case "phone": {
  const [countryCode, setCountryCode] = useState("+40");
  const [phoneNumber, setPhoneNumber] = useState("");
  const [search, setSearch] = useState("");
  const [open, setOpen] = useState(false);
  const dropdownRef = useRef<HTMLDivElement>(null);

  const countries = [
    { code: "+1", name: "United States" },
    { code: "+40", name: "Romania" },
    { code: "+373", name: "Moldova" },
    { code: "+44", name: "United Kingdom" },
    { code: "+91", name: "India" },
  ];

  const filteredCountries = countries.filter(
    (c) =>
      c.name.toLowerCase().includes(search.toLowerCase()) ||
      c.code.includes(search)
  );

  // 🔹 Close dropdown on outside click
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (
        dropdownRef.current &&
        !dropdownRef.current.contains(event.target as Node)
      ) {
        setOpen(false);
      }
    };
    document.addEventListener("mousedown", handleClickOutside);
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, []);

  const handlePhoneChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setPhoneNumber(e.target.value);
    const full = `${countryCode}${e.target.value}`;
    setIsValid(e.target.validity.valid);
    onChange?.({
      ...e,
      target: { ...e.target, value: full, id },
    } as any);
  };

  const handleCountryChange = (c: { code: string; name: string }) => {
    setCountryCode(c.code);
    setOpen(false);
    const full = `${c.code}${phoneNumber}`;
    setIsValid(/^[0-9]{6,14}$/.test(phoneNumber));
    onChange?.({
      target: { value: full, id } as any,
    } as any);
  };

  return (
    <div className={styles.formInput}>
      <label htmlFor={id}>{label}</label>
      <div className={styles.phoneWrapper}>
        {/* Custom Dropdown */}
        <div className={styles.dropdown} ref={dropdownRef}>
          <div
            className={styles.dropdown__header}
            onClick={() => setOpen(!open)}
          >
            <span>{countryCode}</span>
          </div>
          {open && (
            <div className={styles.dropdown__menu}>
              <div className={styles.search}>
                <input
                  type="text"
                  placeholder="Search country..."
                  value={search}
                  onChange={(e) => setSearch(e.target.value)}
                  className={styles.searchInput}
                />
              </div>
              <div className={styles.options}>
                {filteredCountries.map((c) => (
                  <div
                    key={c.code}
                    className={styles.option}
                    onClick={() => handleCountryChange(c)}
                  >
                    {c.name} ({c.code})
                  </div>
                ))}
              </div>
            </div>
          )}
        </div>
        <input
          id={id}
          type="tel"
          placeholder={placeholder ?? "Enter your phone number"}
          required={required}
          value={phoneNumber}
          onChange={handlePhoneChange}
          onBlur={() => setFocused(true)}
          data-focused={focused.toString()}
          pattern="^[0-9]{6,14}$"
        />
      </div>
      {focused && !isValid && (
        <span className="error">Please enter a valid phone number</span>
      )}
    </div>
  );
}

case "calendar":
  const [selectedDate, setSelectedDate] = useState<Date | null>(null);
  const [currentMonth, setCurrentMonth] = useState(new Date());
  const [showCalendar, setShowCalendar] = useState(false);
  const [inputValue, setInputValue] = useState("");
  const [isDateValid, setIsDateValid] = useState(true);
  const popupRef = useRef<HTMLDivElement>(null);
  const wrapperRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (
        wrapperRef.current &&
        !wrapperRef.current.contains(event.target as Node)
      ) {
        setShowCalendar(false);
      }
    };
    document.addEventListener("mousedown", handleClickOutside);
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, []);

  const handleDateClick = (day: number) => {
    const newDate = new Date(
      currentMonth.getFullYear(),
      currentMonth.getMonth(),
      day
    );
    setSelectedDate(newDate);
    setInputValue(newDate.toISOString().split("T")[0]);
    setIsDateValid(true);
    setShowCalendar(false); // close after picking
    onChange?.({
      target: { id, value: newDate.toISOString().split("T")[0] },
    } as any);
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const val = e.target.value;
    setInputValue(val);

    const match = /^(\d{4})-(\d{2})-(\d{2})$/.exec(val);
    if (match) {
      const year = parseInt(match[1]);
      const month = parseInt(match[2]) - 1;
      const day = parseInt(match[3]);

      const newDate = new Date(year, month, day);
      if (
        newDate.getFullYear() === year &&
        newDate.getMonth() === month &&
        newDate.getDate() === day
      ) {
        setSelectedDate(newDate);
        setCurrentMonth(newDate);
        setIsDateValid(true);
        onChange?.({
          target: { id, value: val },
        } as any);
      }
    } else {
      setIsDateValid(false);
    }
  };

  const daysInMonth = new Date(
    currentMonth.getFullYear(),
    currentMonth.getMonth() + 1,
    0
  ).getDate();

  const startDay = new Date(
    currentMonth.getFullYear(),
    currentMonth.getMonth(),
    1
  ).getDay();

  const prevMonth = () => {
    setCurrentMonth(
      new Date(currentMonth.getFullYear(), currentMonth.getMonth() - 1, 1)
    );
  };

  const nextMonth = () => {
    setCurrentMonth(
      new Date(currentMonth.getFullYear(), currentMonth.getMonth() + 1, 1)
    );
  };

  return (
    <div ref={wrapperRef} className={styles.calendarFrom}>
      <label htmlFor={id}>{label}</label>
      <div className={styles.inputWrapper}>
        <input
          id={id}
          type="text"
          placeholder={placeholder ?? "YYYY-MM-DD"}
          value={inputValue}
          onChange={handleInputChange}
          required={required}
          onClick={() => setShowCalendar((prev) => !prev)} // toggle on click
        />
        <span
          className={styles.icon}
          onClick={() => setShowCalendar((prev) => !prev)}
        >
        </span>
      </div>
      {showCalendar && (
        <div ref={popupRef} className={styles.calendarPopup}>
          <div className={styles.header}>
            <button type="button" onClick={prevMonth}>{"<"}</button>
            <span>
              {currentMonth.toLocaleString("default", { month: "long" })}{" "}
              {currentMonth.getFullYear()}
            </span>
            <button type="button" onClick={nextMonth}>{">"}</button>
          </div>

          <div className={styles.weekdays}>
            {["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"].map((day) => (
              <div key={day}>{day}</div>
            ))}
          </div>

          <div className={styles.days}>
            {Array.from({ length: startDay }).map((_, i) => (
              <div key={"empty-" + i} className={styles.empty}></div>
            ))}
            {Array.from({ length: daysInMonth }).map((_, i) => {
              const day = i + 1;
              const isSelected =
                selectedDate &&
                selectedDate.getDate() === day &&
                selectedDate.getMonth() === currentMonth.getMonth() &&
                selectedDate.getFullYear() === currentMonth.getFullYear();

              return (
                <div
                  key={day}
                  className={`${styles.day} ${isSelected ? styles.selected : ""}`}
                  onClick={() => handleDateClick(day)}
                >
                  {day}
                </div>
              );
            })}
          </div>
        </div>
      )}

      {!isDateValid && (
        <span className="error">Please select a valid date</span>
      )}
    </div>
  );

   case "select": {
  const [selectedOption, setSelectedOption] = useState<string>("");
  const [open, setOpen] = useState(false);
  const dropdownRef = useRef<HTMLDivElement>(null);

  const options = [
    { value: "us", label: "United States" },
    { value: "ro", label: "Romania" },
    { value: "md", label: "Moldova" },
    { value: "uk", label: "United Kingdom" },
    { value: "in", label: "India" },
  ];

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target as Node)) {
        setOpen(false);
      }
    };
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  const handleOptionSelect = (option: { value: string; label: string }) => {
    setSelectedOption(option.label);
    setOpen(false);
    onChange?.({
      target: { value: option.value, id },
    } as unknown as React.ChangeEvent<HTMLInputElement>);
  };

  return (
    <div className={styles.formInput}>
      <label htmlFor={id}>{label}</label>
      <div className={styles.selectWrapper} ref={dropdownRef}>
        {/* Dropdown Header with button and arrow */}
        <button
          type="button"
          className={styles.dropdown__header}
          onClick={() => setOpen(!open)}
        >
          <span>{selectedOption || "Select an option"}</span>
          {open?<DropDown style={{transform:"rotate(180deg)"}}/>:<DropDown/>}
        </button>

        {/* Dropdown Menu */}
        {open && (
          <div className={styles.dropdown__menu}>
            {options.map((option) => (
              <div
                key={option.value}
                className={styles.option}
                onClick={() => handleOptionSelect(option)}
              >
                {option.label}
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}
case "user-select": {
      const [selectedUserId, setSelectedUserId] = useState<string>("");
      const [open, setOpen] = useState(false);
      const dropdownRef = useRef<HTMLDivElement>(null);

      useEffect(() => {
        const handleClickOutside = (event: MouseEvent) => {
          if (dropdownRef.current && !dropdownRef.current.contains(event.target as Node)) {
            setOpen(false);
          }
        };
        document.addEventListener("mousedown", handleClickOutside);
        return () => document.removeEventListener("mousedown", handleClickOutside);
      }, []);

      const handleUserSelect = (user: UserOption | null) => {
        const valueToSet = user ? user.id : "";
        setSelectedUserId(valueToSet);
        setOpen(false);
        
        onChange?.({
          target: { value: valueToSet, id },
        } as unknown as React.ChangeEvent<HTMLInputElement>);
      };

      const selectedUser = userOptions?.find(u => u.id === selectedUserId);

      const getProfileImage = (url?: string) => {
        if (!url) return null;
        const cleanUrl = url.replace(/^(\/?\.\.\/)+/, ''); 
        return cleanUrl.startsWith('/') ? cleanUrl : `/${cleanUrl}`;
      };

      return (
        <div className={styles.userSelectForm}>
          <label htmlFor={id}>{label}</label>
          <div className={styles.selectWrapper} ref={dropdownRef}>
          
            <button
              type="button"
              className={styles.dropdown__header}
              onClick={() => setOpen(!open)}
            >
              <div className={styles.assignedBlock}>
                {selectedUser ? (
                 <span>{selectedUser.userName}</span>
                ) : (
                  <span>{placeholder || "Unassigned"}</span>
                )}
              </div>
            </button>
            {open && (
              <div className={styles.dropdown__menu}>
                <div
                  className={styles.option}
                  onClick={() => handleUserSelect(null)}
                  style={{ padding: '8px', cursor: 'pointer', borderBottom: '1px solid var(--border-color)', color: 'var(--text-secondary)' }}
                >
                  Unassigned
                </div>
                {userOptions?.map((user) => (
                  <div
                    key={user.id}
                    className={styles.option}
                    onClick={() => handleUserSelect(user)}
                    style={{ display: 'flex', alignItems: 'center', gap: '8px', padding: '8px', cursor: 'pointer' }}
                  >
                     {user.profilePictureUrl ? (
                      <img 
                        src={getProfileImage(user.profilePictureUrl) as string} 
                        alt={user.userName} 
                        style={{ width: '24px', height: '24px', borderRadius: '50%', objectFit: 'cover' }}
                      />
                    ) : (
                      <div style={{ width: '24px', height: '24px', borderRadius: '50%', backgroundColor: '#555' }} />
                    )}
                    <span>{user.userName}</span>
                  </div>
                ))}
              </div>
            )}
          </div>
        </div>
      );
    }

  case "radio":
  const [selectedPoint, setSelectedPoint] = useState("");

  const handleRadioChange = (value: string) => {
    setSelectedPoint(value);
    setIsValid(!!value); // valid if something is selected
    onChange?.({
      target: { id, value },
    } as any);
  };

  return (
    <div className={styles.formInput}>
      <label>{label}</label>
      <div className="radioGroup">
        {options.map((opt, idx) => (
          <label key={idx} className="radioOption">
            <input
              type="radio"
              name={id} // group by id
              value={opt} // use text value
              checked={selectedPoint === opt} // compare with text
              onChange={() => handleRadioChange(opt)}
              onBlur={() => setFocused(true)}
              required={required}
            />
            {`${idx + 1}. ${opt}`}
          </label>
        ))}
      </div>
      {focused && !isValid && (
        <span className="error">Please select a valid option</span>
      )}
    </div>
  );
  case "textarea":
  const [textValue, setTextValue] = useState("");

  const handleTextChange = (e: React.ChangeEvent<HTMLTextAreaElement >) => {
    setTextValue(e.target.value);
  };

  return (
    <div className={styles.formInput}>
      <label htmlFor={id}>{label}</label>
      <textarea
        id={id}
        value={textValue}
        placeholder={placeholder ?? "Enter text"}
        required={required}
        onChange={handleTextChange}
        onBlur={() => setFocused(true)}
        data-focused={focused.toString()}
      />
      {focused && required && textValue.trim() === "" && (
        <span className="error">This field cannot be empty</span>
      )}
    </div>
  );
    case "text":
    default:
      return (
        <>
        <div className={styles.formInput}>
          <label htmlFor={id}>{label}</label>
          <input
            id={id}
            type="text"
            className={styles.text}
            placeholder={placeholder ?? "Enter text"}
            required={required}
            onChange={handleChange}
            onBlur={() => setFocused(true)}
            data-focused={focused.toString()}
          />
          {focused && !isValid && (
            <span className="error">Please enter valid text</span>
          )}
        </div>
        </>
      );
  }
  
};

export default InputData;
