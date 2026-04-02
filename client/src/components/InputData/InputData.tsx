import { useState, useRef, useEffect } from "react";
import styles from "./InputData.module.scss";
import { Button } from "../Button/Button";
import { ShowPassword, HidePassword, Calendar, DropDown } from "../Icons/Icons";

type UserOption = {
  id: string;
  userName: string;
  profilePictureUrl?: string;
};

type TagOption = {
  id: string;
  name: string;
  colorHex: string;
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
    | "textarea"
    | "tag-select";
  id: string;
  label: string;
  placeholder?: string;
  required?: boolean;
  options?: string[];
  userOptions?: UserOption[];
  tagOptions?: TagOption[];
  selectedTags?: string[];
  onTagToggle?: (tagId: string) => void;
  onCreateTag?: (name: string, colorHex: string) => void;
  value?: string;
  onChange?: (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>
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
  tagOptions = [],
  selectedTags = [],
  onTagToggle,
  onCreateTag,
  onChange,
  value = "",
}: InputDataProps) => {
  const [focused, setFocused] = useState(false);
  const [isValid, setIsValid] = useState(true);
  const [showPassword, setShowPassword] = useState(false);

  // 1. THIS IS THE MAGIC FIX: The input keeps track of its own text
  const [internalValue, setInternalValue] = useState(value);

  // 2. If the parent ever FORCES a value change (like resetting the form), sync it
  useEffect(() => {
    setInternalValue(value);
  }, [value]);

  // 3. Update the internal view instantly, then tell the parent
  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>
  ) => {
    setInternalValue(e.target.value); // Makes typing instantly visible

    if (e.target.validity) {
      setIsValid(e.target.validity.valid);
    }
    onChange?.(e); // Send the data to your form
  };

  switch (type) {
    case "tag-select": {
      const [open, setOpen] = useState(false);
      const [isCreating, setIsCreating] = useState(false);
      const [newTagName, setNewTagName] = useState("");
      const [newTagColor, setNewTagColor] = useState("#734FCF");
      const dropdownRef = useRef<HTMLDivElement>(null);

      useEffect(() => {
        const handleClickOutside = (event: MouseEvent) => {
          if (dropdownRef.current && !dropdownRef.current.contains(event.target as Node)) {
            setOpen(false);
            setIsCreating(false);
          }
        };
        document.addEventListener('mousedown', handleClickOutside);
        return () => document.removeEventListener('mousedown', handleClickOutside);
      }, []);

      const handleCreate = async () => {
        if (!newTagName.trim() || !onCreateTag) return;
        await onCreateTag(newTagName.trim(), newTagColor);
        setNewTagName("");
        setIsCreating(false);
      };

      return (
        <div className={styles.formInput} style={{ position: 'relative' }} ref={dropdownRef}>
          <label htmlFor={id}>{label}</label>
          
          <div style={{ display: 'flex', flexWrap: 'wrap', gap: '6px', padding: '8px', border: '1px solid #444', borderRadius: '4px', background: '#222', minHeight: '40px', alignItems: 'center' }}>
            {selectedTags.map(tagId => {
              const tag = tagOptions.find(t => t.id === tagId);
              if (!tag) return null;
              return (
                <span key={tagId} style={{ backgroundColor: tag.colorHex, color: 'white', padding: '2px 8px', borderRadius: '12px', fontSize: '0.8rem', display: 'flex', alignItems: 'center', gap: '6px' }}>
                  {tag.name}
                  <span style={{ cursor: 'pointer', fontWeight: 'bold' }} onClick={() => onTagToggle?.(tagId)}>×</span>
                </span>
              );
            })}

            <button 
              type="button" 
              onClick={() => setOpen(!open)}
              style={{ background: 'transparent', border: 'none', color: '#888', cursor: 'pointer', fontSize: '0.85rem', padding: '2px 5px' }}
            >
              {selectedTags.length === 0 ? (placeholder || "Select tags...") : "+ Add"}
            </button>
          </div>

          {open && (
            <div style={{ position: 'absolute', top: '100%', left: 0, right: 0, marginTop: '5px', background: '#2a2a2a', border: '1px solid #444', borderRadius: '4px', zIndex: 10, padding: '10px', boxShadow: '0 4px 10px rgba(0,0,0,0.5)' }}>
              <div style={{ display: 'flex', flexDirection: 'column', gap: '5px', maxHeight: '150px', overflowY: 'auto', marginBottom: '10px' }}>
                {tagOptions.filter(t => !selectedTags.includes(t.id)).map(tag => (
                  <div 
                    key={tag.id} 
                    onClick={() => onTagToggle?.(tag.id)}
                    style={{ padding: '6px 10px', display: 'flex', alignItems: 'center', gap: '8px', cursor: 'pointer', borderRadius: '4px' }}
                    onMouseEnter={e => e.currentTarget.style.background = '#3a3a3a'}
                    onMouseLeave={e => e.currentTarget.style.background = 'transparent'}
                  >
                    <div style={{ width: '12px', height: '12px', borderRadius: '50%', backgroundColor: tag.colorHex }}></div>
                    <span style={{ color: 'white', fontSize: '0.9rem' }}>{tag.name}</span>
                  </div>
                ))}
              </div>

              {isCreating ? (
                <div style={{ display: 'flex', flexDirection: 'column', gap: '8px', borderTop: '1px solid #444', paddingTop: '10px' }}>
                  <input 
                    autoFocus
                    placeholder="New tag name..." 
                    value={newTagName}
                    onChange={(e) => setNewTagName(e.target.value)}
                    style={{ padding: '6px', borderRadius: '4px', border: '1px solid #555', backgroundColor: '#111', color: '#fff', fontSize: '0.85rem' }}
                    maxLength={30}
                  />
                  <div style={{ display: 'flex', alignItems: 'center', gap: '8px' }}>
                    <input 
                      type="color" 
                      value={newTagColor}
                      onChange={(e) => setNewTagColor(e.target.value)}
                      style={{ width: '30px', height: '30px', padding: '0', border: 'none', cursor: 'pointer', backgroundColor: 'transparent' }}
                    />
                    <span style={{ color: '#888', fontSize: '0.8rem' }}>Pick a color</span>
                  </div>
                  <div style={{ display: 'flex', gap: '6px' }}>
                    <button type="button" onClick={handleCreate} disabled={!newTagName.trim()} style={{ flex: 1, padding: '6px', backgroundColor: '#734FCF', color: '#fff', border: 'none', borderRadius: '4px', cursor: 'pointer', fontSize: '0.8rem' }}>Save</button>
                    <button type="button" onClick={() => setIsCreating(false)} style={{ flex: 1, padding: '6px', backgroundColor: '#444', color: '#fff', border: 'none', borderRadius: '4px', cursor: 'pointer', fontSize: '0.8rem' }}>Cancel</button>
                  </div>
                </div>
              ) : (
                <button 
                  type="button" 
                  onClick={() => setIsCreating(true)}
                  style={{ width: '100%', padding: '8px', backgroundColor: '#333', color: '#ddd', border: 'none', borderRadius: '4px', cursor: 'pointer', fontSize: '0.85rem', borderTop: '1px solid #444' }}
                >
                  + Create New Tag
                </button>
              )}
            </div>
          )}
        </div>
      );
    }

    case "email":
      return (
        <div className={styles.formInput}>
          <label htmlFor={id}>{label}</label>
          <input
            className={`${styles.email} ${isValid ? "" : styles.error}`}
            id={id}
            type="email"
            value={internalValue} // Use internalValue here
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
        <div className={styles.formInput}>
          <label htmlFor={id}>{label}</label>
          <div className={`${styles.password} ${isValid ? "" : styles.error}`}>
            <input
              id={id}
              type={showPassword ? "text" : "password"}
              value={internalValue} // Use internalValue here
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
              icon={showPassword ? <ShowPassword /> : <HidePassword />}
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
      const [open, setOpen] = useState(false);
      const dropdownRef = useRef<HTMLDivElement>(null);
      
      const countryCodeMatch = internalValue.match(/^(\+\d{1,3})/);
      const countryCode = countryCodeMatch ? countryCodeMatch[1] : "+40";
      const phoneNumber = internalValue.replace(countryCode, "");

      const countries = [
        { code: "+1", name: "United States" },
        { code: "+40", name: "Romania" },
        { code: "+373", name: "Moldova" },
        { code: "+44", name: "United Kingdom" },
        { code: "+91", name: "India" },
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

      const handlePhoneChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const fullNumber = `${countryCode}${e.target.value}`;
        setInternalValue(fullNumber); // Update internally
        onChange?.({
          ...e,
          target: { ...e.target, value: fullNumber, id },
        } as any);
      };

      const handleCountryChange = (c: { code: string; name: string }) => {
        setOpen(false);
        const fullNumber = `${c.code}${phoneNumber}`;
        setInternalValue(fullNumber); // Update internally
        onChange?.({
          target: { value: fullNumber, id } as any,
        } as any);
      };

      return (
        <div className={styles.formInput}>
          <label htmlFor={id}>{label}</label>
          <div className={styles.phoneWrapper}>
            <div className={styles.dropdown} ref={dropdownRef}>
              <div className={styles.dropdown__header} onClick={() => setOpen(!open)}>
                <span>{countryCode}</span>
              </div>
              {open && (
                <div className={styles.dropdown__menu}>
                  <div className={styles.options}>
                    {countries.map((c) => (
                      <div key={c.code} className={styles.option} onClick={() => handleCountryChange(c)}>
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
            />
          </div>
        </div>
      );
    }

    case "calendar": {
      return (
        <div className={styles.formInput}>
          <label htmlFor={id}>{label}</label>
          <input
            id={id}
            type="date"
            value={internalValue} // Use internalValue here
            required={required}
            onChange={handleChange}
            className={styles.calendarInput}
          />
        </div>
      );
    }

    case "select": {
      const selectOptionsList = [
        { value: "us", label: "United States" },
        { value: "ro", label: "Romania" },
        { value: "md", label: "Moldova" },
        { value: "uk", label: "United Kingdom" },
        { value: "in", label: "India" },
      ];

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

      const handleOptionSelect = (option: { value: string; label: string }) => {
        setOpen(false);
        setInternalValue(option.value); // Update internally
        onChange?.({
          target: { value: option.value, id },
        } as unknown as React.ChangeEvent<HTMLInputElement>);
      };

      const displayLabel = selectOptionsList.find((o) => o.value === internalValue)?.label || "Select an option";

      return (
        <div className={styles.formInput}>
          <label htmlFor={id}>{label}</label>
          <div className={styles.selectWrapper} ref={dropdownRef}>
            <button
              type="button"
              className={styles.dropdown__header}
              onClick={() => setOpen(!open)}
            >
              <span>{displayLabel}</span>
              {open ? <DropDown style={{ transform: "rotate(180deg)" }} /> : <DropDown />}
            </button>

            {open && (
              <div className={styles.dropdown__menu}>
                {selectOptionsList.map((option) => (
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
        setOpen(false);
        setInternalValue(valueToSet); // Update internally
        onChange?.({
          target: { value: valueToSet, id },
        } as unknown as React.ChangeEvent<HTMLInputElement>);
      };

      const selectedUser = userOptions?.find(u => u.id === internalValue);

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

    case "radio": {
      return (
        <div className={styles.formInput}>
          <label>{label}</label>
          <div className="radioGroup">
            {options.map((opt, idx) => (
              <label key={idx} className="radioOption">
                <input
                  type="radio"
                  name={id}
                  value={opt}
                  checked={internalValue === opt} // Use internalValue here
                  onChange={handleChange}
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
    }

    case "textarea": {
      return (
        <div className={styles.formInput}>
          <label htmlFor={id}>{label}</label>
          <textarea
            id={id}
            value={internalValue} // Use internalValue here
            placeholder={placeholder ?? "Enter text"}
            required={required}
            onChange={handleChange}
            onBlur={() => setFocused(true)}
            data-focused={focused.toString()}
          />
          {focused && required && internalValue.trim() === "" && (
            <span className="error">This field cannot be empty</span>
          )}
        </div>
      );
    }

    case "text":
    default:
      return (
        <div className={styles.formInput}>
          <label htmlFor={id}>{label}</label>
          <input
            id={id}
            type="text"
            className={styles.text}
            value={internalValue}
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
      );
  }
};

export default InputData;